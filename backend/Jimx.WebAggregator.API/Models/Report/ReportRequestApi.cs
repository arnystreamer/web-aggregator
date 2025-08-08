using Jimx.Common.WebApi.Models;

namespace Jimx.WebAggregator.API.Models.Report;

public class ReportRequestApi : CollectionRequestApi
{
    public ReportRequestApi() : base(0, 100)
    {
    }

    public ReportRequestApi(int? skip,
        int? take,
        int salaryTypeId,
        decimal? manualSalary,
        decimal? salaryMultiplicator,
        int sortingFunction,
        bool? sortAscending) : base(skip, take)
    {
        SalaryTypeId = salaryTypeId;
        ManualSalary = manualSalary;
        SalaryMultiplicator = salaryMultiplicator;
        SortingFunction = sortingFunction;
        SortAscending = sortAscending;
    }

    public required int SalaryTypeId { get; init; }

    public decimal? ManualSalary { get; init; }

    public decimal? SalaryMultiplicator { get; init; }

    public required int SortingFunction { get; init; }

    public bool? SortAscending { get; init; }
}