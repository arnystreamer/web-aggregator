namespace Jimx.WebAggregator.Browser.WebCrawler;

public class JobSearchInterval(string value)
{
    public override string ToString()
    {
        return value;
    }

    public static readonly JobSearchInterval FullDay = new ("r86400");
}