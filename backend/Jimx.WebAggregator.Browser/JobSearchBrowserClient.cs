using Jimx.WebAggregator.Browser.WebCrawler;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Jimx.WebAggregator.Browser;

public class JobSearchBrowserClient : IAsyncDisposable
{
    private readonly ILogger _logger;
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    
    public JobSearchBrowserClient(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task StartAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions 
            {
                Headless = false
            });
    }

    public JobSearchBrowserContext GetJobSearchContext(JobSearchBrowserContextOptions jobSearchBrowserContextOptions)
    {
        if (_playwright == null)
            throw new InvalidOperationException("Playwright is not initialized");
        
        if (_browser == null)
            throw new InvalidOperationException("Browser is not initialized");
        
        return new JobSearchBrowserContext(_logger, _browser, jobSearchBrowserContextOptions);
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
            await _browser.DisposeAsync();
        }

        _playwright?.Dispose();
    }
}