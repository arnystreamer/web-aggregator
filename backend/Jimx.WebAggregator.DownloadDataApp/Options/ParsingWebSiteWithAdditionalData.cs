namespace Jimx.WebAggregator.DownloadDataApp.Options;

public class ParsingWebSiteWithAdditionalData<TAdditionalData> : ParsingWebSite
{
    public required TAdditionalData AdditionalData { get; set; }
}