namespace Jimx.WebAggregator.Domain.CityCosts;

public record CityDataItem(string Key, decimal? Value, decimal? Low, decimal? High)
{
	public int? DictionaryId { get; set; }
}