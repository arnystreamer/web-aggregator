namespace Jimx.WebAggregator.API.Models.Report;

public class ReportCityExtendedApi : ReportCityApi
{
    public ReportCityExtendedApi(string name, string region, string country, string? countryCode) : base(name, region, country, countryCode)
    {
    }

    public required ReportProfitTaxable SelectedSalary { get; init; }
    public required SalaryData SalaryData { get; init; }
    public required string CurrencyCode { get; init; }
    public required bool HasFreeApartment { get; init; }

    public required decimal GoingOutCosts { get; init; }
    public required decimal GroceriesCosts { get; init; }
    public required decimal HouseholdCosts { get; init; }
    public required decimal TransportationCosts { get; init; }
    public required decimal UtilitiesCosts { get; init; }
    public required decimal SportsAndLeisureCosts { get; init; }
    public required decimal ClothingCosts { get; init; }
    public required decimal RentCosts { get; init; }
    public required decimal MortgageCosts { get; init; }
    public required decimal ChildcareCosts { get; init; }
    public required decimal VacationCosts { get; init; }
    public required decimal ElectronicsCosts { get; init; }

    public required ReportCostRelativeExtended MinimumCostsWithRent { get; init; }
    public required ReportCostRelativeExtended CostsWithRent { get; init; }
    
    public required ReportCostRelativeExtended CostsWithMortgage { get; init; }
    
    public required MultiCurrencyValue MonthlySavingsWhileRenting { get; init; }
    public required MultiCurrencyValue MonthlySavingsWhilePayingMortgage { get; init; }
    
    public required ReportProfitTargetTerm MillionaireTerm { get; init; }
    public required ReportProfitTargetTerm MortgageDownPaymentTerm { get; init; }
    public required ReportProfitTargetTerm BuyCarTerm { get; init; }
    
    public required ReportProfitTaxableRelativeDesirable SalaryToEarn1MlnUsdIn30Yrs { get; init; }
    public required ReportProfitTaxableRelativeDesirable SustainableSalary { get; init; }
    public required ReportProfitTaxableRelativeDesirable BareMinimumSalary { get; init; }
}