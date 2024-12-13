using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB
{
	public abstract class MongoUnitOfWork<TCollectionItem> : IDisposable
	{
		public abstract void Do(IMongoCollection<TCollectionItem> mongoCollection);

		public abstract void Dispose();
	}
}
