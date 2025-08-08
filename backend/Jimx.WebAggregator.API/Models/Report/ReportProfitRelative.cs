namespace Jimx.WebAggregator.API.Models.Report;

public class ReportProfitRelative : ReportProfit
{
    public decimal BaseValueGross { get; }
    public decimal FractionValue => BaseValueGross != 0m ? ValueGross.Value / BaseValueGross : 0;
    public decimal DifferenceValue => ValueGross.Value - BaseValueGross;
    
    public ReportProfitRelative(decimal valueGross, decimal valueNet, decimal crossRateToUsd, decimal baseValueGross) 
        : base(valueGross, valueNet, crossRateToUsd)
    {
        if (baseValueGross < 0)
        {
            throw new ArgumentException(nameof(baseValueGross));
        }
        
        BaseValueGross = baseValueGross;
    }
}