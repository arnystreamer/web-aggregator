namespace Jimx.WebAggregator.API.Models.CityCosts
{
	public class CityDataItemApi
	{
		public string Key { get; set; }
		public string DictionaryId { get; set; }
		public string Value { get; set; }
		public decimal? DecimalValue => Decimal.TryParse(Value, out var result) ? result : null;
		
	}
}
