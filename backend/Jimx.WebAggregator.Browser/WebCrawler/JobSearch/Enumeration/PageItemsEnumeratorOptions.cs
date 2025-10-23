using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Specs;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;

public record PageItemsEnumeratorOptions(JobSearchItemExpandSpecification ExpandSpecification, int MinimalWaitAfterNetworkOperation);