using Jimx.WebAggregator.API.Options;
using Jimx.WebAggregator.Domain.CityCosts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Jimx.WebAggregator.API.Services
{
	public class CitiesDatabaseService
	{
		private readonly IOptions<CitiesDatabaseSettings> _databaseSettings;

		public CitiesDatabaseService(IOptions<CitiesDatabaseSettings> databaseSettings)
		{
			_databaseSettings = databaseSettings;
		}

		public List<City> GetCities()
		{
			var client = new MongoClient(_databaseSettings.Value.ConnectionString);

			var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
			var collection = database.GetCollection<City>(_databaseSettings.Value.CitiesCollectionName);

			return collection.Find(_ => true).ToList();
						
		}

		public List<DataItem> GetDataItems()
		{
			var client = new MongoClient(_databaseSettings.Value.ConnectionString);
			
			var database = client.GetDatabase(_databaseSettings.Value.DatabaseName);
			var collection = database.GetCollection<DataItem>(_databaseSettings.Value.CityDictionaryItemsCollectionName);

			return collection.Find(_ => true).ToList();
			
		}
	}
}
