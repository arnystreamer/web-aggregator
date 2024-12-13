using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations
{
	public abstract class MongoUnitOfWork<TCollectionItem> : IDisposable
	{
		public abstract IEnumerable<TCollectionItem> Do(IMongoCollection<TCollectionItem> mongoCollection);

		public abstract void Dispose();
	}
}
