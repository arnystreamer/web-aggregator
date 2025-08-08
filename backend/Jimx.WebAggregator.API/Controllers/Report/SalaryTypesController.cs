using Jimx.WebAggregator.API.Models.Report;
using Jimx.WebAggregator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jimx.WebAggregator.API.Controllers.Report;

[Route("api/salary-types")]
[ApiController]
public class SalaryTypesController : ControllerBase
{
    private readonly ILogger<SalaryTypesController> _logger;
    private readonly SalaryTypesService _salaryTypesService;

    public SalaryTypesController(ILogger<SalaryTypesController> logger, SalaryTypesService salaryTypesService)
    {
        _logger = logger;
        _salaryTypesService = salaryTypesService;
    }

    [HttpGet]
    public SalaryTypeApi[] Get([FromQuery] SalaryTypesRequestApi requestApi)
    {
        return _salaryTypesService.GetAll()
            .Skip(requestApi.Skip ?? 0)
            .Take(requestApi.Take ?? 10)
            .ToArray();
    }
}