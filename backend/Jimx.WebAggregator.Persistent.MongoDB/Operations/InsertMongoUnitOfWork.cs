using Jimx.WebAggregator.Domain.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations
{
	public class InsertMongoUnitOfWork<TCollectionItem, TIdentity> : MongoUnitOfWork<TCollectionItem>
		where TCollectionItem : IMongoEntity
	{
		private readonly IEnumerable<TCollectionItem> _updatedItems;
		private readonly InsertOptions<TCollectionItem, TIdentity> _insertOptions;

		public InsertMongoUnitOfWork(IEnumerable<TCollectionItem> updatedItems, InsertOptions<TCollectionItem, TIdentity> insertOptions)
		{
			_updatedItems = updatedItems;
			_insertOptions = insertOptions;
		}

		public override IEnumerable<TCollectionItem> Do(ILogger logger, IMongoCollection<TCollectionItem> mongoCollection)
		{
			var existingItems = mongoCollection.Find(i => true).ToList();

			foreach (var updatedItem in _updatedItems)
			{
				var existingItem = existingItems.SingleOrDefault(i => _insertOptions.IdentityComparer.Equals(i, updatedItem));

				if (existingItem == null)
				{
					logger.LogInformation($"{mongoCollection.CollectionNamespace.FullName} MongoDb collection: inserting new item");
					mongoCollection.InsertOne(updatedItem);
				}

				yield return updatedItem;
			}
		}

		public override void Dispose()
		{

		}
	}
}
