namespace Jimx.WebAggregator.API.Models.Report;

public class SortingFunction(int id, string functionName, string name, string? description, Func<SortingDirection, IComparer<ReportCityExtendedApi>> sortingComparer)
{
    public int Id { get; } = id;
    public string FunctionName { get; } = functionName;
    public string Name { get; } = name;
    public string? Description { get; } = description;
    public Func<SortingDirection, IComparer<ReportCityExtendedApi>> SortingComparer { get; } = sortingComparer;
}