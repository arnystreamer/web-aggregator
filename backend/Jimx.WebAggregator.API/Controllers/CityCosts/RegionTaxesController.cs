using Jimx.Common.WebApi.Models;
using Jimx.WebAggregator.API.Models.CityCosts;
using Jimx.WebAggregator.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Jimx.WebAggregator.API.Controllers.CityCosts
{
	[Route("api/region-taxes")]
	[ApiController]
	public class RegionTaxesController : ControllerBase
	{
		private readonly ILogger<RegionTaxesController> _logger;
		private readonly CitiesDatabaseService _databaseService;

		public RegionTaxesController(ILogger<RegionTaxesController> logger, CitiesDatabaseService databaseService)
		{
			_logger = logger;
			_databaseService = databaseService;
		}

		[HttpGet]
		public async Task<CollectionApi<RegionTaxApi>> GetAll([FromQuery] CollectionRequestApi requestApi)
		{
			int skip = requestApi.Skip ?? 0;
			int take = requestApi.Take ?? 10;

			var allItems = _databaseService.GetRegionTaxes();

			var dataItems =
				allItems
				.Skip(skip)
				.Take(take)
				.Select(i => new RegionTaxApi(i.Region, i.Country, i.Fixed, i.FixedRate,
					i.Levels.Select(l => new TaxLevelApi(l.LowerCut, l.Rate)).ToArray()))
				.ToArray();

			return new CollectionApi<RegionTaxApi>(
				allItems.Count(), skip, take, dataItems.Length,
				dataItems);
		}

		[HttpGet("fakes")]
		public async Task<CollectionApi<RegionTaxApi>> GetFake()
		{
			var allItems = _databaseService.GetCities().GroupBy(c => ( c.Region, c.Country ));

			var dataItems =
				allItems
				.Select(i => new RegionTaxApi(i.Key.Region, i.Key.Country, 0, 0,
					[ new (0, 0), new(0, 0), new(0, 0) ]
					))
				.ToArray();

			return new CollectionApi<RegionTaxApi>(
				allItems.Count(), 0, dataItems.Count(), dataItems.Length,
				dataItems);
		}
	}
}
