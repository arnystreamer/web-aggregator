namespace Jimx.WebAggregator.Calculations.Models;

public class AppliedTaxesResult
{
    public decimal SalaryGross { get; }
    
    public TaxResult[] TaxResults { get; }
    public decimal TotalDeductions => TaxResults.Sum(t => t.Value);
    public decimal SalaryNet => SalaryGross >= TotalDeductions ? SalaryGross - TotalDeductions : 0m;

    public AppliedTaxesResult(decimal salaryGross, TaxResult[] taxResults)
    {
        SalaryGross = salaryGross;
        TaxResults = taxResults;
    }
}