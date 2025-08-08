namespace Jimx.WebAggregator.API.Models.Report;

public class SalaryData
{
    public required ReportProfitTaxable AverageSalary { get; init; }
    public required ReportProfitTaxable P25Salary { get; init; }
    public required ReportProfitTaxable P75Salary { get; init; }
}