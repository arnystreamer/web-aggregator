using JetBrains.Annotations;
using Jimx.WebAggregator.Calculations.Models;

namespace Jimx.WebAggregator.API.Models.Report;

[PublicAPI]
public class ReportProfitTaxableRelative : ReportProfitTaxable
{
    public decimal BaseValueGross { get; }
    public decimal FractionValue => BaseValueGross != 0m ? ValueGross.Value / BaseValueGross : 0;
    public decimal DifferenceValue => ValueGross.Value - BaseValueGross;
    
    public ReportProfitTaxableRelative(AppliedTaxesResult appliedTaxesResult, decimal crossRateToUsd, decimal baseValueGross) 
        : base(appliedTaxesResult, crossRateToUsd)
    {
        if (baseValueGross < 0)
        {
            throw new ArgumentException(null, nameof(baseValueGross));
        }
        
        BaseValueGross = baseValueGross;
    }    
    
    public ReportProfitTaxableRelative(decimal salaryGross, decimal salaryNet, decimal crossRateToUsd, TaxBit[] taxBits, decimal baseValueGross) 
        : base(salaryGross, salaryNet, crossRateToUsd, taxBits)
    {
        if (baseValueGross < 0)
        {
            throw new ArgumentException(null, nameof(baseValueGross));
        }
        
        BaseValueGross = baseValueGross;
    }

    public ReportProfitTaxableRelative(ReportProfitTaxable reportProfitTaxable, decimal baseValueGross)
        :base(reportProfitTaxable.ValueNet, reportProfitTaxable.ValueGross, reportProfitTaxable.TaxBits)
    {
        BaseValueGross = baseValueGross;
    }
}