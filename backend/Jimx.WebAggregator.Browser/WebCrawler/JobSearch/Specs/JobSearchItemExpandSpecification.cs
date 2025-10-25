using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Specs;

public abstract class JobSearchItemExpandSpecification
{
    public abstract bool NeedsExpand(JobListItem item);
}