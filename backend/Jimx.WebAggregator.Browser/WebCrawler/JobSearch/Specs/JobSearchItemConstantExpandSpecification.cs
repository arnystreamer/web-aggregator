using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Specs;

public class JobSearchItemConstantExpandSpecification(bool needsExpand) : JobSearchItemExpandSpecification
{
    public override bool NeedsExpand(JobListItem item)
    {
        return needsExpand;
    }
}