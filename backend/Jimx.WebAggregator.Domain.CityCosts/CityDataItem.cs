namespace Jimx.WebAggregator.Domain.CityCosts
{
	public record CityDataItem(string Key, string? Value)
	{
		public string? DictionaryId { get; set; }
	};
}
