using Microsoft.AspNetCore.Mvc;

namespace WebAggregator.External.API.Controllers
{
	[Route("api/Version")]
	public class VersionController : Controller
	{
		[HttpGet]
		public string Get()
		{
			return "1.0.0.0";
		}
	}
}