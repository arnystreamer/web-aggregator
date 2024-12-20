namespace Jimx.WebAggregator.API.Options
{
	public class CitiesDatabaseSettings
	{
		public const string OptionName = "CitiesDatabase";

		public string ConnectionString { get; set; } = string.Empty;
		public string DatabaseName { get; set; } = string.Empty;

		public string CitiesCollectionName { get; set; } = string.Empty;
		public string CityDictionaryItemsCollectionName { get; set; } = string.Empty;
		public string RegionTaxesCollectionName { get; set; } = string.Empty;
		public string CitySalariesCollectionName { get; set; } = string.Empty;
	}
}