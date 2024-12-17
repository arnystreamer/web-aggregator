namespace Jimx.WebAggregator.API.Models.CityCosts
{
	public record CityApi(string Id, string Name, string Region, string Country, CityDataItemApi[] DataItems);
}
