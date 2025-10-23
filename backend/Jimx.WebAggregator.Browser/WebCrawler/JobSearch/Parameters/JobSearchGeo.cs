namespace Jimx.WebAggregator.Browser.WebCrawler;

public class JobSearchGeo(string value)
{
    public override string ToString()
    {
        return value;
    }
    
    public static readonly JobSearchGeo Switzerland = new ("106693272");
}