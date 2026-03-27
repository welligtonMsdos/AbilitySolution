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
            Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
        });

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Iniciando extração MSN: {time}", DateTimeOffset.Now);

                var page = await browser.NewPageAsync();
                
                await page.GotoAsync("https://www.msn.com/pt-br", new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle,
                    Timeout = 60000
                });
                
                await page.WaitForSelectorAsync("a#heading", new PageWaitForSelectorOptions { Timeout = 10000 });

                var cards = await page.QuerySelectorAllAsync("a#heading");

                foreach (var card in cards.Take(10))
                {
                    try
                    {
                        var titulo = await card.EvaluateAsync<string>("el => el.innerText");
                        var url = await card.EvaluateAsync<string>("el => el.href");
                      
                        if (!string.IsNullOrEmpty(url) && !url.StartsWith("http"))
                            url = "https://www.msn.com" + (url.StartsWith("/") ? "" : "/") + url;

                        if (!string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrEmpty(url))
                        {
                            if (!await _repository.JaExisteUrlAsync(url))
                            {
                                var news = new Noticia { Titulo = titulo.Trim(), Url = url };
                                await _repository.SalvarNoticiaAsync(news);
                                _logger.LogInformation("Nova notícia capturada: {title}", titulo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Erro ao processar um card específico: {msg}", ex.Message);
                    }
                }
               
                await page.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha crítica na rodada de extração.");
            }
            
            _logger.LogInformation("Aguardando 5 minutos para a próxima varredura...");

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
