using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations;

public abstract class MongoUnitOfWork<TCollectionItem> : IDisposable
{
	public abstract Task<IEnumerable<TCollectionItem>> DoAsync(ILogger logger, IMongoCollection<TCollectionItem> mongoCollection);

	public abstract void Dispose();
}