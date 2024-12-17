using Jimx.WebAggregator.API.Models.CityCosts;
using Jimx.WebAggregator.API.Models.Common;
using Jimx.WebAggregator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jimx.WebAggregator.API.Controllers.CityCosts
{
	[Route("api/city-salaries")]
	[ApiController]
	public class CitySalariesController : ControllerBase
	{
		private readonly ILogger<CitiesController> _logger;
		private readonly CitiesDatabaseService _databaseService;

		public CitySalariesController(ILogger<CitiesController> logger, CitiesDatabaseService databaseService)
		{
			_logger = logger;
			_databaseService = databaseService;
		}

		[HttpGet]
		public async Task<CollectionApi<CitySalaryApi>> GetAll([FromQuery] CollectionRequestApi requestApi)
		{
			int skip = requestApi.Skip ?? 0;
			int take = requestApi.Take ?? 10;

			var allItems = _databaseService.GetCitySalaries();

			var dataItems =
				allItems
				.Skip(skip)
				.Take(take)
				.Select(i => new CitySalaryApi(i.City, i.P25, i.Average, i.P75))
				.ToArray();

			return new CollectionApi<CitySalaryApi>(
				allItems.Count(), skip, take, dataItems.Length,
				dataItems);
		}
	}
}
