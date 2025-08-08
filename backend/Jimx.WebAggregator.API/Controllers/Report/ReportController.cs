using Jimx.Common.WebApi.Models;
using Jimx.WebAggregator.API.Models.Report;
using Jimx.WebAggregator.API.Services;
using Jimx.WebAggregator.Calculations;
using Microsoft.AspNetCore.Mvc;

namespace Jimx.WebAggregator.API.Controllers.Report;

[Route("api/report")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly ReportService _reportService;
    private readonly SortingFunctionsService _sortingFunctionsService;

    public ReportController(ReportService reportService, SortingFunctionsService sortingFunctionsService)
    {
        _reportService = reportService;
        _sortingFunctionsService = sortingFunctionsService;
    }

    [HttpGet]
    public async Task<CollectionApi<ReportCityExtendedApi>> Get([FromQuery] ReportRequestApi requestApi, CancellationToken cancellationToken)
    {
        var sortingFunction = _sortingFunctionsService.Get(requestApi.SortingFunction);

        var userTaxProfile = new UserTaxProfile(
            [],
            new UserFamily()
            {
                FamilyMembersCount = 3,
                ToddlersCount = 1,
                PrescholarsCount = 0,
                ScholarsCount = 0
            });
        
        var reportCityApis = await _reportService.Get(
            requestApi.SalaryTypeId, requestApi.ManualSalary, requestApi.SalaryMultiplicator, sortingFunction, 
            requestApi.SortAscending == false ? SortingDirection.Descending : SortingDirection.Ascending, 
            userTaxProfile,
            cancellationToken);
        
        return new CollectionApi<ReportCityExtendedApi>(reportCityApis.Length, 0, reportCityApis.Length, reportCityApis.Length,  reportCityApis);
    }
}