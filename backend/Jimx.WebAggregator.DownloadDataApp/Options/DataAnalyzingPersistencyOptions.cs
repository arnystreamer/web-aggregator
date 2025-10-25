namespace Jimx.WebAggregator.DownloadDataApp.Options;

public class DataAnalyzingPersistencyOptions
{
    public required string DatabaseName { get; set; }
    public required string LeftCollection { get; set; }
    public required string RightCollection { get; set; }
    
}