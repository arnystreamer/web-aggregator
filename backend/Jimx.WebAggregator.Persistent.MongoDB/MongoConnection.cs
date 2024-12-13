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

		public void DoWork<TUnitOfWork, TCollectionItem>(TUnitOfWork unit)
			where TUnitOfWork : MongoUnitOfWork<TCollectionItem>
		{
			MongoClient client = new MongoClient(_options.ConnectionString);
			using (var session = client.StartSession())
			{
				session.StartTransaction();
				try
				{
					var database = client.GetDatabase(_options.DatabaseName);
					var collection = database.GetCollection<TCollectionItem>(_options.CollectionName);

					unit.Do(collection);

					session.CommitTransaction();
				}
				catch
				{
					session.AbortTransaction();
				}
			}
		}
	}
}
