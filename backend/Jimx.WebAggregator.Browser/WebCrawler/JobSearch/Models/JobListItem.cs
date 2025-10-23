using Jimx.WebAggregator.Browser.WebCrawler.Page;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;

public record JobListItem(string Link, string Title, bool IsVerified, string CompanyName, 
    string Location, string? LocationFormat, string? PublishedAgoString, string? ViewedString, bool IsEasyApply, string Html) : HtmlSnapshot(Html);