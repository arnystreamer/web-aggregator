namespace Jimx.WebAggregator.API.Options;

public class CitiesDatabaseSettings
{
	public const string OptionName = "CitiesDatabase";

	public string ConnectionString { get; init; } = string.Empty;
	public string DatabaseName { get; init; } = string.Empty;

	public string CitiesCollectionName { get; init; } = string.Empty;
	public string CityDictionaryItemsCollectionName { get; init; } = string.Empty;
	public string RegionTaxesCollectionName { get; init; } = string.Empty;
	public string RegionTaxDeductionsCollectionName { get; init; } = string.Empty;
	public string CitySalariesCollectionName { get; init; } = string.Empty;
}