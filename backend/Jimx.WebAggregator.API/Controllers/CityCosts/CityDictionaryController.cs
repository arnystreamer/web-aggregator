using Jimx.Common.WebApi.Models;
using Jimx.WebAggregator.API.Models.CityCosts;
using Jimx.WebAggregator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jimx.WebAggregator.API.Controllers.CityCosts
{
	[Route("api/city-dictionary")]
	[ApiController]
	public class CityDictionaryController : ControllerBase
	{
		private readonly ILogger<CityDictionaryController> _logger;
		private readonly CitiesDatabaseService _databaseService;

		public CityDictionaryController(ILogger<CityDictionaryController> logger, CitiesDatabaseService databaseService)
		{
			_logger = logger;
			_databaseService = databaseService;
		}

		[HttpGet]
		public async Task<CollectionApi<DictionaryDataApi>> GetAll([FromQuery] CityDictionaryRequestApi requestApi,
			CancellationToken cancellationToken)
		{
			int skip = requestApi.Skip ?? 0;
			int take = requestApi.Take ?? 10;

			var allItems = await _databaseService.GetCityDictionaryItemsAsync(cancellationToken);

			var dataItems = 
				allItems
				.Skip(skip)
				.Take(take)
				.Select(i => new DictionaryDataApi(i.ObjectId!, i.Key, i.Value))
				.ToArray();

			return new CollectionApi<DictionaryDataApi>(
				allItems.Count(), skip, take, dataItems.Length,
				dataItems);
		}
	}
}
