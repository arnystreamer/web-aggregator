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

	public async Task<List<CityCostsItem>> GetCityCostsAsync(int year, int month, CancellationToken cancellationToken)
	{
		var client = new MongoClient(_databaseSettings.Value.ConnectionString);

		var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
		var collection = database.GetCollection<CityCostsItem>(_databaseSettings.Value.CitiesCollectionName);

		return (await collection.FindAsync(c => c.Year == year && c.Month == month, cancellationToken: cancellationToken)).ToList();
	}

	public async Task<List<CityCostsItem>> GetLatestCityCostsAsync(CancellationToken cancellationToken)
	{
		var client = new MongoClient(_databaseSettings.Value.ConnectionString);

		var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
		var collection = database.GetCollection<CityCostsItem>(_databaseSettings.Value.CitiesCollectionName);

		var allItems = (await collection.FindAsync(_ => true, cancellationToken: cancellationToken)).ToList();

		return allItems.GroupBy(i => new { i.Name, i.Region, i.Country })
			.Select(g => g.OrderByDescending(i => i.Year).ThenByDescending(i => i.Month).First())
			.ToList();
	}

	public async Task<CityDataTimeStamp[]> GetCityDataTimeStampsAsync(CancellationToken cancellationToken)
	{
		var client = new MongoClient(_databaseSettings.Value.ConnectionString);

		var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
		var collection = database.GetCollection<CityCostsItem>(_databaseSettings.Value.CitiesCollectionName);
		
		var allTimeStamps = (await collection.FindAsync(_ => true, cancellationToken: cancellationToken)).ToList()
			.GroupBy(c => new CityDataTimeStamp(c.Year, c.Month))
			.OrderBy(g => g.Key)
			.Select(g => g.Key)
			.ToArray();

		return allTimeStamps;
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