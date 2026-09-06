using Jimx.WebAggregator.Domain.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations;

public class GetMongoUnitOfWork<TCollectionItem> : MongoUnitOfWork<TCollectionItem>
    where TCollectionItem : IMongoEntity
{
    public override async Task<IEnumerable<TCollectionItem>> DoAsync(ILogger logger, IMongoCollection<TCollectionItem> mongoCollection)
    {
        return [];
    }

    public override void Dispose()
    {
        
    }
}