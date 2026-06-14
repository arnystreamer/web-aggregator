namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Parameters;

public class JobSearchGeo(string value)
{
    public override string ToString()
    {
        return value;
    }
    
    public static readonly JobSearchGeo Germany = new ("101282230");
    public static readonly JobSearchGeo Berlin = new ("103035651");
    public static readonly JobSearchGeo Netherlands = new ("102890719");
    public static readonly JobSearchGeo EuropeanUnion = new ("91000000");
    public static readonly JobSearchGeo Vienna = new ("107144641");
    public static readonly JobSearchGeo Australia = new ("101452733");
    public static readonly JobSearchGeo Sydney = new ("104769905");
    public static readonly JobSearchGeo UnitedStates = new ("103644278");
    
}