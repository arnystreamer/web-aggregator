using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;
using Jimx.WebAggregator.Browser.WebCrawler.Page;

namespace Jimx.WebAggregator.Browser.WebCrawler;

public record JobSearchBrowserContextOptions(
    string BaseUrl,
    string? UserAgent,
    SessionInitializationOptions SessionInitializationOptions,
    PagingEnumeratorOptions PagingEnumeratorOptions);