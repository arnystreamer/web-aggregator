namespace Jimx.WebAggregator.API.Models.Report;

public class ReportCostRelativeExtended : ReportCostExtended
{
    public ReportProfitTaxable BaseProfit { get; }
    public decimal FractionValue => BaseProfit.ValueGross.Value != 0m ? ValueNet.Value / BaseProfit.ValueGross.Value : 0;
    public decimal DifferenceValue => ValueNet.Value - BaseProfit.ValueGross.Value;
    
    public ReportCostRelativeExtended(decimal valueNet, decimal crossRateToUsd, CostBit[] costBits, ReportProfitTaxable baseProfit) 
        : base(valueNet, crossRateToUsd, costBits)
    {
        BaseProfit = baseProfit;
    }

    public ReportCostRelativeExtended(CostBit[] costBits, decimal crossRateToUsd, ReportProfitTaxable baseProfit) 
        : this(costBits.Sum(b => b.Value), crossRateToUsd, costBits, baseProfit)
    {
        
    }
}