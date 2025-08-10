namespace Jimx.WebAggregator.Calculations.Models;

public class AppliedTaxesResult(decimal salaryGross, TaxResult[] taxResults)
{
    public decimal SalaryGross { get; } = salaryGross;

    public TaxResult[] TaxResults { get; } = taxResults;
    public decimal TotalDeductions => TaxResults.Sum(t => t.Value);
    public decimal SalaryNet => SalaryGross >= TotalDeductions ? SalaryGross - TotalDeductions : 0m;
}