namespace Jimx.WebAggregator.Domain.CityCosts;

public record CityDataItem(string Key, decimal? Value)
{
	public int? DictionaryId { get; set; }
}