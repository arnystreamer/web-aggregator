using Jimx.WebAggregator.Browser.WebCrawler.Page;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;

public record JobListDetails(string Html) : HtmlSnapshot(Html);