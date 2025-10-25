namespace Jimx.WebAggregator.DownloadDataApp.Options;

public class LifeLevelAdditionalData
{
    public required LifeLevelAdditionalDataRequestSettings CommonCostRequestSettings { get; set; }
    public required LifeLevelAdditionalDataRequestSettings PropertyInvestmentRequestSettings { get; set; }
    public required ParsingInitialDataRow[] InitialCityRows { get; set; }
}