using Ability.Domain.Entities;
using Ability.Domain.Interfaces;
using Microsoft.Playwright;

namespace Ability.Worker.Work;

public class RpaWorkService: BackgroundService
{
    private readonly ILogger<RpaWorkService> _logger;
    private readonly IRpaRepository _repository;

    public RpaWorkService(ILogger<RpaWorkService> logger,
                          IRpaRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {        
        using var playwright = await Playwright.CreateAsync();

        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
        {
            Headless = true,
            Args = new[] {
                "--no-sandbox",
                "--disable-setuid-sandbox",
                "--disable-dev-shm-usage",
                "--disable-gpu"
            }
        });

        while (!stoppingToken.IsCancellationRequested)
        {   
            IPage? page = null;
            try
            {
                _logger.LogInformation("Iniciando extração MSN: {time}", DateTimeOffset.Now);

                page = await browser.NewPageAsync();
               
                await page.GotoAsync("https://www.msn.com/pt-br", new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded,
                    Timeout = 60000
                });
               
                await page.WaitForSelectorAsync("a#heading", new() { State = WaitForSelectorState.Attached, Timeout = 30000 });
                
                var cards = await page.QuerySelectorAllAsync("a#heading");
                _logger.LogInformation("Encontrados {count} cards para processar.", cards.Count);

                foreach (var card in cards.Take(10))
                {                   
                    if (page.IsClosed) break;

                    try
                    {
                        var titulo = await card.InnerTextAsync();

                        var url = await card.GetAttributeAsync("href");

                        if (!string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrEmpty(url))
                        {
                            if (!url.StartsWith("http"))
                                url = "https://www.msn.com" + (url.StartsWith("/") ? "" : "/") + url;

                            if (!await _repository.JaExisteUrlAsync(url))
                            {
                                var news = new Noticia { Titulo = titulo.Trim(), Url = url };
                                await _repository.SalvarNoticiaAsync(news);
                                _logger.LogInformation("Nova notícia capturada: {title}", titulo.Trim());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Erro ao processar um card específico: {msg}", ex.Message);
                    }                    
                }
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, "Falha crítica na rodada de extração.");
             
                if (page != null)
                    await page.ScreenshotAsync(new() { Path = $"erro_{DateTime.Now:ticks}.png" });
            }
            finally
            {                
                if (page != null) await page.CloseAsync();
            }

            _logger.LogInformation("Aguardando 5 minutos para a próxima varredura...");

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }

        await browser.CloseAsync();
    }
}
