using Jimx.WebAggregator.Persistent.MongoDB.Operations;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB
{
	public class MongoConnection
	{
		private readonly MongoOptions _options;

		public MongoConnection(MongoOptions options)
		{
			_options = options;
		}

		public DoWorkResult<TCollectionItem> DoWork<TUnitOfWork, TCollectionItem>(TUnitOfWork unit)
			where TUnitOfWork : MongoUnitOfWork<TCollectionItem>
		{
			MongoClient client = new MongoClient(_options.ConnectionString);
			using (var session = client.StartSession())
			{
				try
				{
					var database = client.GetDatabase(_options.DatabaseName);
					var collection = database.GetCollection<TCollectionItem>(_options.CollectionName);

					_options.Logger.LogInformation($"{typeof(TUnitOfWork)} job starting");
					var affectedItems = unit.Do(_options.Logger, collection);
					_options.Logger.LogInformation($"{typeof(TUnitOfWork)} job finished");

					var allItems = collection.Find(_ => true).ToList();

					return new DoWorkResult<TCollectionItem>(false, allItems, affectedItems.ToList());
				}
				catch
				{
					return new DoWorkResult<TCollectionItem>(true, [], []);
				}
			}
		}

		public record DoWorkResult<TCollectionItem>(bool IsFailure, IEnumerable<TCollectionItem> AllItems, IEnumerable<TCollectionItem> AffectedItems);
	}
}
