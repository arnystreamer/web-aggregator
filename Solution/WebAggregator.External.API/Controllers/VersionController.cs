using Microsoft.AspNetCore.Mvc;
using WebAggregator.External.API.Models;

namespace WebAggregator.External.API.Controllers
{
	[Route("api/Version")]
	public class VersionController : Controller
	{
		[HttpGet]
		public Version Get()
		{
			return new Version("WebAggregator.External.API", "1.0.0.0");
		}
	}
}