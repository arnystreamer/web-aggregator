namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Parameters;

public class JobSearchLocations(JobSearchLocation[] locations)
{
    public override string ToString()
    {
        return string.Join(",", locations.Select(l => (int)l));
    }
}