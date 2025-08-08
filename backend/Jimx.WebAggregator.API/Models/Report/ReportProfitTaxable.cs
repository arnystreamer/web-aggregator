using Jimx.WebAggregator.Calculations.Models;

namespace Jimx.WebAggregator.API.Models.Report;

public class ReportProfitTaxable
{
    public MultiCurrencyValue ValueGross { get; }

    public MultiCurrencyValue ValueNet { get; } 
    
    public TaxBit[] TaxBits { get; } 
    
    public decimal TotalDeductions => TaxBits.Sum(t => t.Value);

    public ReportProfitTaxable(AppliedTaxesResult appliedTaxesResult, decimal crossRateToUsd)
        :this(appliedTaxesResult.SalaryGross, appliedTaxesResult.SalaryNet, crossRateToUsd, 
            appliedTaxesResult.TaxResults.Select(t => new TaxBit(t.Name, t.Value)).ToArray())
    {

    }

    public ReportProfitTaxable(decimal salaryGross, decimal salaryNet, decimal crossRateToUsd, TaxBit[] taxBits)
        :this(
            new MultiCurrencyValue(salaryGross, salaryGross * crossRateToUsd),
            new MultiCurrencyValue(salaryNet, salaryNet * crossRateToUsd),
            taxBits)
    {
        
    }

    public ReportProfitTaxable(MultiCurrencyValue valueGross, MultiCurrencyValue valueNet, TaxBit[] taxBits)
    {
        ValueGross = valueGross;
        ValueNet = valueNet;
        TaxBits = taxBits;
    }
}