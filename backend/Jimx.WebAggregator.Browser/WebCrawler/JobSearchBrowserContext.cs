using System.Web;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Parameters;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Jimx.WebAggregator.Browser.WebCrawler;

public class JobSearchBrowserContext : IAsyncDisposable
{
    private readonly ILogger _logger;
    private readonly IBrowser _browser;
    private readonly JobSearchBrowserContextOptions _options;
    private IBrowserContext? _browserContext;
    private Cookie[]? _initialCookies;

    public JobSearchBrowserContext(ILogger logger, IBrowser browser, JobSearchBrowserContextOptions options)
    {
        _logger = logger;
        _browser = browser;
        _options = options;
    }

    public async Task<IBrowserContext> StartBrowsingContextAsync()
    {
        try
        {
            if (_browserContext != null)
            {
                _logger.LogWarning("Browser context already started");
                return _browserContext;
            }

            _browserContext = await _browser.NewContextAsync(
                new BrowserNewContextOptions
                {
                    ViewportSize = new ViewportSize() { Width = 1600, Height = 900 },
                    UserAgent = _options.UserAgent
                }
            );
            
            _logger.LogInformation("Browser context started");

            await _browserContext.ClearCookiesAsync();

            var sessionOptions = _options.SessionInitializationOptions;
            _initialCookies = sessionOptions.Cookies;

            await _browserContext.AddCookiesAsync(
                _initialCookies
                    .Select(c => new Microsoft.Playwright.Cookie()
                    {
                        Name = c.Name,
                        Value = c.Value,
                        Domain = c.Domain,
                        Path = c.Path,
                        Expires = c.ExpiresInMs
                    })
                    .ToArray()
            );
            
            _logger.LogDebug("Browser context was fulfilled with cookies");

            return _browserContext;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Browser context creation failed: {ExceptionMessage}", e.Message);
            throw;
        }
    }

    public async Task<PagingEnumerable?> OpenPageAsync(JobSearchParameters searchParameters)
    {
        try
        {
            if (_browserContext == null)
            {
                _logger.LogWarning("Browser context not started");
                return null;
            }

            var page = await _browserContext.NewPageAsync();
            _logger.LogDebug("New page started");

            var url = new Uri(_options.BaseUrl, UriKind.Absolute);

            var query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("f_TPR", searchParameters.Interval.ToString());
            if (searchParameters.IsEarlyApplicant == true)
            {
                query.Add("f_EA", "true");
            }

            query.Add("f_WT", searchParameters.Locations.ToString());
            query.Add("keywords", searchParameters.Keywords);
            query.Add("geoId", searchParameters.Geo.ToString());
            query.Add("origin", "JOB_SEARCH_PAGE_JOB_FILTER");

            switch (searchParameters.SortBy)
            {
                case JobSearchSorting.MostRecent:
                    query.Add("sortBy", "DD");
                    break;
                case JobSearchSorting.Relevance:
                    query.Add("sortBy", "R");
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var fullUrl = $"{url}?{query}";
            _logger.LogDebug("Url built: {FullUrl}", fullUrl);

            await page.GotoAsync(
                fullUrl,
                new PageGotoOptions()
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });
            
            _logger.LogInformation("Got page and loading finished.");

            var jobsContainerLocator = page.Locator("div.scaffold-layout__list-detail-inner");
            return new PagingEnumerable(_logger, jobsContainerLocator, _options.PagingEnumeratorOptions);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Opening page failed: {ExceptionMessage}", e.Message);
            throw;
        }
    }

    public async Task<CookieDiff[]> GetCookiesAsync()
    {
        if (_browserContext == null)
        {
            throw new InvalidOperationException("Browser has not been started.");
        }

        if (_initialCookies == null)
        {
            throw new InvalidOperationException("Browser has not been started: initial cookies are empty");
        }

        var browserCookies = await _browserContext.CookiesAsync();

        var newCookies = browserCookies
            .Select(c => new Cookie(c.Name, c.Value, c.Domain, c.Path, c.Expires))
            .ToArray();
        
        var allNames = _initialCookies.Select(c => c.Name).Union(newCookies.Select(c => c.Name)).ToArray();
        var oldC = _initialCookies.ToDictionary(c => c.Name);
        var newC = newCookies.ToDictionary(c => c.Name);

        return allNames
            .Select(n => new CookieDiff(
                n,
                !oldC.ContainsKey(n) || !newC.ContainsKey(n) || oldC[n].Value != newC[n].Value,
                oldC.GetValueOrDefault(n),
                newC.GetValueOrDefault(n)
            ))
            .ToArray();
    }
    
    public async ValueTask DisposeAsync()
    {
        if (_browserContext != null)
        {
            await _browserContext.DisposeAsync();
        }
    }
}