namespace Jimx.WebAggregator.DownloadDataApp.Options;

public class ParsingWebSiteOptions
{
    public required string BaseUrl { get; set; }
    public required string UserAgent { get; set; }
    public required ParsingCookiesOptions CookiesOptions { get; set; }
    public required float MinimalWaitAfterNetworkOperationInMs { get; set; }
}