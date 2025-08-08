using Jimx.WebAggregator.API.Models;
using Jimx.WebAggregator.API.Models.Report;
using Jimx.WebAggregator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jimx.WebAggregator.API.Controllers.Report;

[Route("api/sorting-functions")]
[ApiController]
public class SortingFunctionsController : ControllerBase
{
    private readonly ILogger<SortingFunctionsController> _logger;
    private readonly SortingFunctionsService _sortingFunctionsService;

    public SortingFunctionsController(ILogger<SortingFunctionsController> logger, SortingFunctionsService sortingFunctionsService)
    {
        _logger = logger;
        _sortingFunctionsService = sortingFunctionsService;
    }

    [HttpGet]
    public SortingFunctionApi[] Get([FromQuery] SortingFunctionsRequestApi requestApi)
    {
        return _sortingFunctionsService.GetAll()
            .Skip(requestApi.Skip ?? 0)
            .Take(requestApi.Take ?? 10)
            .Select(x => new SortingFunctionApi(x.Id, x.FunctionName, x.Name, x.Description))
            .ToArray();
    }
}