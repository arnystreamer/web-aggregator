using Microsoft.AspNetCore.Mvc;

namespace Jimx.WebAggregator.API.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        return "healthy";
    }
}