namespace Jimx.WebAggregator.API.Models.Report;

public class ReportProfitTargetTerm
{
    public ReportProfitTaxable Income { get; }
    public ReportCostExtended Cost { get; }
    public MultiCurrencyValue TargetAmount { get; }
    public decimal? TermInMonths { get; }
    public decimal? TermInYears => TermInMonths / 12m;

    public ReportProfitTargetTerm(ReportProfitTaxable income, ReportCostExtended cost, MultiCurrencyValue targetAmount, decimal? termInMonths)
    {
        Income = income;
        Cost = cost;
        TargetAmount = targetAmount;
        TermInMonths = termInMonths;
    }
}