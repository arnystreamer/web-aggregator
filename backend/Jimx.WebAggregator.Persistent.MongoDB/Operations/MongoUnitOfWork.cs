using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations
{
	public abstract class MongoUnitOfWork<TCollectionItem> : IDisposable
	{
		[Obsolete]
		public abstract IEnumerable<TCollectionItem> Do(ILogger logger, IMongoCollection<TCollectionItem> mongoCollection);
		public abstract Task<IEnumerable<TCollectionItem>> DoAsync(ILogger logger, IMongoCollection<TCollectionItem> mongoCollection);

		public abstract void Dispose();
	}
}
