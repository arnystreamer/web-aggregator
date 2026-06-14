using Jimx.WebAggregator.Browser;
using Jimx.WebAggregator.Browser.WebCrawler;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Parameters;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Specs;
using Jimx.WebAggregator.Domain.JobNet;
using Jimx.WebAggregator.DownloadDataApp.Options;
using Jimx.WebAggregator.Parser;
using Jimx.WebAggregator.Persistent.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.DownloadDataApp;

public class JobNetParsingJob : IParsingJob
{
    private readonly ILogger _logger;
    private readonly ParsingWebSiteOptions _options;
    private readonly PersistencyOptionsBundle<JobNetPersistencyOptions> _persistencyOptions;

    private MongoConnection.CollectionConnection<JobVacancyItem>? _jobsConnection;

    public JobNetParsingJob(ILogger logger, ParsingWebSiteWithAdditionalData<JobNetAdditionalData> options, PersistencyOptionsBundle<JobNetPersistencyOptions> persistencyOptions)
    {
        _logger = logger;
        _options = options.Options;
        _persistencyOptions = persistencyOptions;
    }

    public static string ConfigurationName => "JobNet";

    public async Task DoAsync()
    {
        var searchParameters = new JobSearchParameters(
            new JobSearchInterval("30m"),
            new JobSearchLocations([JobSearchLocation.Onsite, JobSearchLocation.Hybrid]),
            ".net developer",
            JobSearchGeo.EuropeanUnion,
            true,
            JobSearchSorting.Relevance
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
        
        const int maxPages = 3;
        int currentPage = 0;
        await foreach (var pageJobItems in pages)
        {
            _logger.LogInformation("Waiting for 10 seconds before processing new page");
            await Task.Delay((int)_options.MinimalWaitAfterNetworkOperationInMs);
            
            await foreach (var jobItem in pageJobItems)
            {
                _logger.LogInformation("Processing list item {JobId}", jobItem.Id);
                await ProcessJobItem(jobItem);
                _logger.LogInformation("Processed successfully list item {JobId}", jobItem.Id);
            }

            currentPage++;
            if (currentPage >= maxPages)
            {
                return;
            }
        }

        var cookiesChanges = (await searchContext.GetCookiesAsync()).Where(cc => cc.IsChanged).ToList();

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

    private MongoConnection.CollectionConnection<JobVacancyItem> GetOrInitializeMongoConnection()
    {
        if (_jobsConnection == null)
        {
            var server = _persistencyOptions.Main.Server;
            var databaseName = _persistencyOptions.Subsection.DatabaseName;

            var mongoCitiesOpts = new MongoOptions(_logger, server, databaseName, _persistencyOptions.Subsection.JobCollection);
            var jobsConnection = new MongoConnection(mongoCitiesOpts).GetCollectionConnection<JobVacancyItem>();
            
            _jobsConnection = jobsConnection;
        }

        return _jobsConnection;
    }

    private async Task ProcessJobItem(JobItem jobItem)
    {
        var approximateCreateTime = DateTime.UtcNow;

        if (jobItem.ListItem.PublishedAgoString != null)
        {
            string[] timeWords = ["hour", "hours", "day", "days", "week", "weeks"];
            var parts = jobItem.ListItem.PublishedAgoString.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var timeCount = 0;
            if (parts.Length != 3 || !int.TryParse(parts[0], out timeCount) || timeWords.Contains(parts[1].Trim()) || parts[2].Trim() != "ago")
            {
                switch (parts[1])
                {
                    case "hour":
                    case "hours":
                        approximateCreateTime -= TimeSpan.FromHours(timeCount);
                        break;
                    case "day":
                    case "days":
                        approximateCreateTime -= TimeSpan.FromDays(timeCount);
                        break;
                    case "week":
                    case "weeks":
                        approximateCreateTime -= TimeSpan.FromDays(timeCount * 7);
                        break;
                }

            }
        }
        else
        {
            approximateCreateTime -= TimeSpan.FromDays(1);
        }

        int? location = null;

        if (jobItem.ListItem.LocationFormat != null)
        {
            location = jobItem.ListItem.LocationFormat switch
            {
                "On-site" => (int)JobSearchLocation.Onsite,
                "Hybrid" => (int)JobSearchLocation.Hybrid,
                "Remote" => (int)JobSearchLocation.Remote,
                _ => location
            };
        }

        var databaseItem = new JobVacancyItem(jobItem.Id, jobItem.ListItem.Link, jobItem.ListItem.Title, 
            jobItem.ListItem.CompanyName, jobItem.ListItem.Location, location, 
            new JobVacancyAuxInfo(jobItem.ListItem.IsVerified, approximateCreateTime, jobItem.ListItem.IsEasyApply, jobItem.ListItem.BeAnEarlyApplicant,
                jobItem.ListItem.IsPromoted, jobItem.ListItem.Metadata),
            null);
        
        var jobsCollection = GetOrInitializeMongoConnection().Collection;
        
        var exisingJob = jobsCollection.Find(j => j.ExternalId == jobItem.Id).FirstOrDefault();
        if (exisingJob == null)
        {
            await jobsCollection.InsertOneAsync(databaseItem);
        }
        else
        {
            await jobsCollection.ReplaceOneAsync(j => j.ExternalId == jobItem.Id, databaseItem);
        }
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