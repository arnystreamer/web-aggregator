using Jimx.Common.WebApi.Models;
using Jimx.WebAggregator.API.Models;
using Jimx.WebAggregator.API.Models.CityCosts;
using Jimx.WebAggregator.API.Services;
using Jimx.WebAggregator.Domain.CityCosts;
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
		public async Task<CollectionApi<RegionTaxApi>> GetAll([FromQuery] RegionTaxesRequestApi requestApi, CancellationToken cancellationToken)
		{
			var (skip, take) = requestApi.ToSkipTake();

			var allItems = await _databaseService.GetRegionTaxesAsync(cancellationToken);

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

		[HttpGet("extended")]
		public async Task<CollectionApi<RegionTaxDeduction>> GetRegionTaxDeductions(
			CancellationToken cancellationToken)
		{
			var (skip, take) = new CollectionRequestApi(0, 1000).ToSkipTake();

			var allItems = await _databaseService.GetRegionTaxDeductionsAsync(cancellationToken);

			var dataItems = allItems.Skip(skip).Take(take).ToArray();
			
			return new CollectionApi<RegionTaxDeduction>(allItems.Count, skip, take, dataItems.Length, dataItems);
		}

		[HttpGet("fakes")]
		public async Task<CollectionApi<RegionTaxApi>> GetFake(CancellationToken cancellationToken)
		{
			var allItems = (await _databaseService.GetCityCostsAsync(cancellationToken))
				.GroupBy(c => ( c.Region, c.Country ))
				.ToArray();

			var dataItems =
				allItems
				.Select(i => new RegionTaxApi(i.Key.Region, i.Key.Country, 0, 0,
					[ new TaxLevelApi(0, 0), new TaxLevelApi(0, 0), new TaxLevelApi(0, 0) ]
					))
				.ToArray();

			return new CollectionApi<RegionTaxApi>(
				allItems.Length, 0, dataItems.Length, dataItems.Length,
				dataItems);
		}
	}
}
