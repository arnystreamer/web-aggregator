namespace Jimx.WebAggregator.API.Models.CityCosts
{
	public record RegionTaxApi(string Region, string Country, decimal Fixed, TaxLevelApi[] Levels);
}
