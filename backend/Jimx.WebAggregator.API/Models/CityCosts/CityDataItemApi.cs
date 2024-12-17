namespace Jimx.WebAggregator.API.Models.CityCosts
{
	public record CityDataItemApi(string Key, string DictionaryId, string Value)
	{
		public decimal? DecimalValue => Decimal.TryParse(Value, out var result) ? result : null;

	}
}
