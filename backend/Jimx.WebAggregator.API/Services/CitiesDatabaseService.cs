using Jimx.WebAggregator.API.Options;
using Jimx.WebAggregator.Domain.CityCosts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Jimx.WebAggregator.API.Services;

public class CitiesDatabaseService
{
	private readonly IOptions<CitiesDatabaseSettings> _databaseSettings;

	public CitiesDatabaseService(IOptions<CitiesDatabaseSettings> databaseSettings)
	{
		_databaseSettings = databaseSettings;
	}

	public async Task<List<CityCostsItem>> GetCityCostsAsync(CancellationToken cancellationToken)
	{
		var client = new MongoClient(_databaseSettings.Value.ConnectionString);

		var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
		var collection = database.GetCollection<CityCostsItem>(_databaseSettings.Value.CitiesCollectionName);

		return (await collection.FindAsync(_ => true, cancellationToken: cancellationToken)).ToList();
						
	}

	public async Task<List<CityDictionaryItem>> GetCityDictionaryItemsAsync(CancellationToken cancellationToken)
	{
		var client = new MongoClient(_databaseSettings.Value.ConnectionString);
			
		var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
		var collection = database.GetCollection<CityDictionaryItem>(_databaseSettings.Value.CityDictionaryItemsCollectionName);

		return (await collection.FindAsync(_ => true, cancellationToken: cancellationToken)).ToList();
			
	}

	public async Task<List<RegionTax>> GetRegionTaxesAsync(CancellationToken cancellationToken)
	{
		var client = new MongoClient(_databaseSettings.Value.ConnectionString);

		var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
		var collection = database.GetCollection<RegionTax>(_databaseSettings.Value.RegionTaxesCollectionName);

		return (await collection.FindAsync(_ => true, cancellationToken: cancellationToken)).ToList();
	}

	public async Task<List<RegionTaxDeduction>> GetRegionTaxDeductionsAsync(CancellationToken cancellationToken)
	{
		var client = new MongoClient(_databaseSettings.Value.ConnectionString);
			
		var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
		var collection =
			database.GetCollection<RegionTaxDeduction>(_databaseSettings.Value.RegionTaxDeductionsCollectionName);
			
		return (await collection.FindAsync(_ => true, cancellationToken: cancellationToken)).ToList();
	}

	public async Task<List<CitySalary>> GetCitySalariesAsync(CancellationToken cancellationToken)
	{
		var client = new MongoClient(_databaseSettings.Value.ConnectionString);

		var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
		var collection = database.GetCollection<CitySalary>(_databaseSettings.Value.CitySalariesCollectionName);

		return (await collection.FindAsync(_ => true, cancellationToken: cancellationToken)).ToList();

	}
}