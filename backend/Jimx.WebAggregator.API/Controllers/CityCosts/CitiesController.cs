using Jimx.WebAggregator.API.Models.CityCosts;
using Jimx.WebAggregator.API.Models.Common;
using Jimx.WebAggregator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jimx.WebAggregator.API.Controllers.CityCosts
{
	[Route("api/cities")]
	[ApiController]
	public class CitiesController : ControllerBase
	{
		private readonly ILogger<CitiesController> _logger;
		private readonly CitiesDatabaseService _databaseService;

		public CitiesController(ILogger<CitiesController> logger, CitiesDatabaseService databaseService)
		{
			_logger = logger;
			_databaseService = databaseService;
		}

		[HttpGet]
		public async Task<CollectionApi<CityApi>> GetAll([FromQuery] CollectionRequestApi requestApi)
		{
			int skip = requestApi.Skip ?? 0;
			int take = requestApi.Take ?? 10;

			var allItems = _databaseService.GetCities();

			var dataItems =
				allItems
				.Skip(skip)
				.Take(take)
				.Select(i => new CityApi(i.ObjectId, i.Name, i.Region, i.Country, i.DataItems.Select(di => new CityDataItemApi(di.Key, di.DictionaryId, di.Value))
					.ToArray()))
				.ToArray();

			return new CollectionApi<CityApi>(
				allItems.Count(), skip, take, dataItems.Length,
				dataItems);
		}
	}
}
