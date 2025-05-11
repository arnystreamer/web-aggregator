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

		public CollectionConnection<TCollectionItem> GetCollectionConnection<TCollectionItem>()
		{
			MongoClient client = new MongoClient(_options.ConnectionString);
			var database = client.GetDatabase(_options.DatabaseName);
			var collection = database.GetCollection<TCollectionItem>(_options.CollectionName);

			return new CollectionConnection<TCollectionItem>(_options.Logger, collection);
		}

		public async Task<DoWorkResult<TCollectionItem>> DoWorkAsync<TUnitOfWork, TCollectionItem>(CollectionConnection<TCollectionItem> collectionConnection, TUnitOfWork unit)
			where TUnitOfWork : MongoUnitOfWork<TCollectionItem>
		{
			try
			{
				_options.Logger.LogInformation($"{typeof(TUnitOfWork)} job starting");
				var affectedItems = await unit.DoAsync(_options.Logger, collectionConnection.Collection);
				_options.Logger.LogInformation($"{typeof(TUnitOfWork)} job finished");

				var allItems = (await collectionConnection.Collection.FindAsync(_ => true)).ToList();

				return new DoWorkResult<TCollectionItem>(false, allItems, affectedItems.ToList());
			}
			catch
			{
				return new DoWorkResult<TCollectionItem>(true, [], []);
			}
			
		}

		public class CollectionConnection<TCollectionItem>
		{
			public CollectionConnection(ILogger logger, IMongoCollection<TCollectionItem> collection)
			{
				Logger = logger;
				Collection = collection;
			}

			public ILogger Logger { get; }
			public IMongoCollection<TCollectionItem> Collection { get; }
		}

		public record DoWorkResult<TCollectionItem>(bool IsFailure, IEnumerable<TCollectionItem> AllItems, IEnumerable<TCollectionItem> AffectedItems);
	}
}
