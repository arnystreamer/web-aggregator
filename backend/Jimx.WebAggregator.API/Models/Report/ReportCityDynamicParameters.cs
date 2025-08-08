namespace Jimx.WebAggregator.API.Models.Report;

public class ReportCityDynamicParameters
{
    public required ReportApartmentParameters ApartmentParameters { get; init; }
    public required decimal SelectedSalary { get; init; }
    public required decimal AnnualGrossSalary { get; init; }
    public required decimal DeveloperGrossSalaryP25 { get; init; }
    public required decimal DeveloperGrossSalaryP75 { get; init; }
    public required ReportHouseholdMembersParameters HouseholdMembers { get; init; }
    public required string CurrencyCode { get; init; }
    public required decimal CrossRateToUsd { get; init; }
    public required bool ArePricesNominatedInUsd { get; init; }
}