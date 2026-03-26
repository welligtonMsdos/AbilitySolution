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
                _logger.LogInformation("Starting MSN extraction: {time}", DateTimeOffset.Now);

                var page = await browser.NewPageAsync();

                await page.GotoAsync("https://www.msn.com/pt-br", new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle,
                    Timeout = 60000
                });

                var cards = await page.QuerySelectorAllAsync("div[data-testid='card-container'], section.content-card");

                foreach (var card in cards.Take(10))
                {
                    try
                    {
                        var element = await card.QuerySelectorAsync("h3");

                        var titulo = (await element?.InnerTextAsync() ?? "").Trim();

                        var link = await card.QuerySelectorAsync("a");

                        var url = await link?.GetAttributeAsync("href") ?? "";

                        if (!string.IsNullOrEmpty(url) && !url.StartsWith("http"))
                            url = "https://www.msn.com" + url;

                        if (!string.IsNullOrWhiteSpace(titulo) && !string.IsNullOrEmpty(url))
                        {
                            if (!await _repository.JaExisteUrlAsync(url))
                            {
                                var news = new Noticia { Titulo = titulo, Url = url };

                                await _repository.SalvarNoticiaAsync(news);

                                _logger.LogInformation("New story captured {title}", titulo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Error processing a specific card: {msg}", ex.Message);
                    }

                    _logger.LogInformation("Waiting for next execution...");

                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical failure in extraction round.");
            }
        }
    }
}
