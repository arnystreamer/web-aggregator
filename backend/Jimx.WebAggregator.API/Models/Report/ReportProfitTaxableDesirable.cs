using Jimx.WebAggregator.Calculations.Models;

namespace Jimx.WebAggregator.API.Models.Report;

public class ReportProfitTaxableRelativeDesirable : ReportProfitTaxableRelative
{
    public MultiCurrencyValue RequestedAnnualIncome { get; }
    
    public CostBit[] CostBits { get; }
    
    public MultiCurrencyValue AnnualSavings { get; }

    public ReportProfitTaxableRelativeDesirable(AppliedTaxesResult appliedTaxesResult, decimal crossRateToUsd, decimal baseValueGross,
        MultiCurrencyValue requestedAnnualIncome, CostBit[] costBits, MultiCurrencyValue annualSavings)
        : base(appliedTaxesResult, crossRateToUsd, baseValueGross)
    {
        RequestedAnnualIncome = requestedAnnualIncome;
        CostBits = costBits;
        AnnualSavings = annualSavings;
    }
}