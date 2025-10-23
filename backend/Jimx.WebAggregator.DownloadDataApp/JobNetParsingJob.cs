using Jimx.WebAggregator.Browser;
using Jimx.WebAggregator.Browser.WebCrawler;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Specs;
using Jimx.WebAggregator.DownloadDataApp.Options;
using Jimx.WebAggregator.Parser;
using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.DownloadDataApp;

public class JobNetParsingJob : IParsingJob
{
    private readonly ILogger _logger;
    private readonly ParsingWebSiteOptions _options;

    public JobNetParsingJob(ILogger logger, ParsingWebSiteOptions options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task DoAsync()
    {
        var searchParameters = new JobSearchParameters(
            JobSearchInterval.FullDay,
            new JobSearchLocations([JobSearchLocation.Onsite, JobSearchLocation.Hybrid]),
            "software developer",
            JobSearchGeo.Switzerland
        );
        
        JobSearchBrowserClient browserClient = new JobSearchBrowserClient(_logger);
        await browserClient.StartAsync();
        var contextOptions = FromConfig(_options);
        
        var searchContext = browserClient.GetJobSearchContext(contextOptions);
        await searchContext.StartBrowsingContextAsync();
        var pages = await searchContext.OpenPageAsync(searchParameters);

        if (pages == null)
        {
            _logger.LogWarning("No results found");
            return;
        }
        
        await foreach (var pageJobItems in pages)
        {
            _logger.LogInformation("Waiting for 10 seconds before processing new page");
            await Task.Delay((int)_options.MinimalWaitAfterNetworkOperationInMs);
            
            await foreach (var jobItem in pageJobItems)
            {
                _logger.LogInformation("Processing list item {JobId}", jobItem.Id);
                ProcessJobItem(jobItem);
                _logger.LogInformation("Processed successfully list item {JobId}", jobItem.Id);
            }
            
            return;
        }

        var cookiesChanges = (await searchContext.GetCookiesAsync()).Where(cc => cc.IsChanged);

        if (cookiesChanges.Any())
        {
            _logger.LogWarning("Some cookies changed during browser operation, consider editing config");
            foreach (var cookiesChange in cookiesChanges)
            {
                if (cookiesChange.NewValue != null)
                {
                    if (cookiesChange.OldValue != null)
                    {
                        _logger.LogInformation("Replaced {CookieName}: {CookieValue}", cookiesChange.Name, cookiesChange.NewValue.Value);
                    }
                    else
                    {
                        _logger.LogInformation("Added {CookieName}: {CookieValue}", cookiesChange.Name, cookiesChange.NewValue.Value);
                    }
                }
                else
                {
                    _logger.LogInformation("Deleted {CookieName}", cookiesChange.Name);                    
                }
            }
        }
    }

    private void ProcessJobItem(JobItem jobItem)
    {
        
    }

    private JobSearchBrowserContextOptions FromConfig(ParsingWebSiteOptions options)
    {
        var cookies = options.CookiesOptions.Cookies
            .Select(c => new Cookie(
                c.Name, 
                c.Value, 
                _options.CookiesOptions.CommonDomain,
                _options.CookiesOptions.CommonPath,
                _options.CookiesOptions.CommonExpirationInMs))
            .ToArray();

        if (options.CookiesOptions.CollateralCookies != null)
        {
            cookies = cookies
                .Union(
                    options.CookiesOptions.CollateralCookies
                        .Select(c => new Cookie(
                            c.Name,
                            c.Value,
                            c.Domain,
                            c.Path,
                            _options.CookiesOptions.CommonExpirationInMs
                        ))
                )
                .ToArray();
        }
        
        var sessionOptions = new SessionInitializationOptions(cookies);

        return new JobSearchBrowserContextOptions(
            _options.BaseUrl,
            _options.UserAgent,
            sessionOptions,
            new PagingEnumeratorOptions(
                (int)_options.MinimalWaitAfterNetworkOperationInMs,
                new PageItemsEnumeratorOptions(
                    new JobSearchItemConstantExpandSpecification(false),
                    (int)_options.MinimalWaitAfterNetworkOperationInMs)
            ));
    }
}