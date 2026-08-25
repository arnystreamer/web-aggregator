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
    private readonly ILogger<ReportController> _logger;
    private readonly ReportService _reportService;
    private readonly SortingFunctionsService _sortingFunctionsService;
    private readonly SalaryTypesService _salaryTypesService;

    public ReportController(ILogger<ReportController> logger, ReportService reportService, SortingFunctionsService sortingFunctionsService,
        SalaryTypesService salaryTypesService)
    {
        _logger = logger;
        _reportService = reportService;
        _sortingFunctionsService = sortingFunctionsService;
        _salaryTypesService = salaryTypesService;
    }

    [HttpGet]
    public async Task<CollectionApi<ReportCityExtendedApi>> Get([FromQuery] ReportRequestApi requestApi, CancellationToken cancellationToken)
    {
        var sortingFunction = _sortingFunctionsService.Get(requestApi.SortingFunctionId);
        var salaryType = _salaryTypesService.GetAll().Single(s => s.Id == requestApi.SalaryTypeId);

        var userTaxProfile = new UserTaxProfile(
            [
                "taxpayer_status:married", 
                "taxpayer_age:36"
            ],
            new UserFamily()
            {
                FamilyMembersCount = 3,
                ToddlersCount = 1,
                PrescholarsCount = 0,
                ScholarsCount = 0
            });
        
        _logger.LogInformation("Get report data for SalaryType={SalaryTypeId} ({SalaryTypeName}), ManualSalary={ManualSalary}, SalaryMultiplicator={SalaryMultiplicator}, "+
            "SortingFunction={SortingFunctionId}({SortingFunctionName}), SortAscending={SortAscending}", 
            salaryType.Id, salaryType.Name, requestApi.ManualSalary, requestApi.SalaryMultiplicator, sortingFunction.Id, sortingFunction.FunctionName, 
            requestApi.SortAscending);
        
        var reportCityApis = await _reportService.Get(
            requestApi.SalaryTypeId, requestApi.ManualSalary, requestApi.SalaryMultiplicator, sortingFunction, 
            requestApi.SortAscending == false ? SortingDirection.Descending : SortingDirection.Ascending, 
            userTaxProfile,
            cancellationToken);
        
        _logger.LogInformation("Get data returned {ReportCityApisCount} elements", reportCityApis.Length);
        
        return new CollectionApi<ReportCityExtendedApi>(reportCityApis.Length, 0, reportCityApis.Length, reportCityApis.Length,  reportCityApis);
    }
}