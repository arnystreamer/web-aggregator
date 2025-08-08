namespace Jimx.WebAggregator.API.Models.Report;

public class ReportCostExtended : ReportCost
{
    public CostBit[] CostBits { get; }
    
    public ReportCostExtended(decimal valueNet, decimal crossRateToUsd, CostBit[] costBits) : base(valueNet, crossRateToUsd)
    {
        CostBits = costBits;
    }
}