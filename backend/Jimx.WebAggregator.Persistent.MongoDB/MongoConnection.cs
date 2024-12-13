using Jimx.WebAggregator.Persistent.MongoDB.Operations;
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

					var affectedItems = unit.Do(collection);

					var allItems = collection.Find(_ => true).ToList();

					return new DoWorkResult<TCollectionItem>(false, allItems, affectedItems);
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
