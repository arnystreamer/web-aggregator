namespace Jimx.WebAggregator.API.Models.Report;

public class ReportHouseholdMembersParameters
{
    public required int TotalCount { get; init; }
    public required int Toddlers { get; init; }
    public required int Prescholars { get; init; }
    public required int Scholars { get; init; }
}