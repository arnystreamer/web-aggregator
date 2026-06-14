namespace Jimx.WebAggregator.DownloadDataApp.Options;

public class JobNetAdditionalData
{
    public required NameValue[] Intervals { get; set; }
    public required NameValue[] Geos { get; set; }
}