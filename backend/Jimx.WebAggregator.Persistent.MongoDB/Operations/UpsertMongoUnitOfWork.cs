using Jimx.WebAggregator.Domain.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations
{
	public class UpsertMongoUnitOfWork<TCollectionItem, TIdentity> : MongoUnitOfWork<TCollectionItem>
		where TCollectionItem : IMongoEntity
	{
		private readonly IEnumerable<TCollectionItem> _updatedItems;
		private readonly UpsertOptions<TCollectionItem, TIdentity> _upsertOptions;

		public UpsertMongoUnitOfWork(IEnumerable<TCollectionItem> updatedItems, UpsertOptions<TCollectionItem, TIdentity> upsertOptions)
		{
			_updatedItems = updatedItems;
			_upsertOptions = upsertOptions;
		}

		public override IEnumerable<TCollectionItem> Do(ILogger logger, IMongoCollection<TCollectionItem> mongoCollection)
		{
			var existingItems = mongoCollection.Find(i => true).ToList();
			var touchedItemIndices = new List<int>();

			foreach (var updatedItem in _updatedItems)
			{
				var existingItem = existingItems.SingleOrDefault(i => _upsertOptions.IdentityComparer.Equals(i, updatedItem));

				if (existingItem == null)
				{
					logger.LogInformation($"{mongoCollection.CollectionNamespace.FullName} MongoDb collection: inserting new item");
					mongoCollection.InsertOne(updatedItem);
				}
				else
				{
					touchedItemIndices.Add(existingItems.IndexOf(existingItem));

					var isExistingItemExpired = (_upsertOptions.ActualityComparer?.Compare(existingItem, updatedItem) ?? -1) < 0;

					if (isExistingItemExpired)
					{
						var selectorExpression = _upsertOptions.IdentityComparerExpression(_upsertOptions.IdentitySelector(updatedItem));
						updatedItem.ObjectId = existingItem.ObjectId;

						logger.LogInformation($"{mongoCollection.CollectionNamespace.FullName} MongoDb collection: replacing existing item {existingItem.ObjectId}");
						mongoCollection.ReplaceOne(selectorExpression, updatedItem);
					}
				}

				yield return updatedItem;
			}

			for (var i = 0; i < existingItems.Count; i++)
			{
				if (!touchedItemIndices.Contains(i))
				{
					var selectorExpression = _upsertOptions.IdentityComparerExpression(_upsertOptions.IdentitySelector(existingItems[i]));

					logger.LogInformation($"{mongoCollection.CollectionNamespace.FullName} MongoDb collection: deleting item {existingItems[i].ObjectId}");
					mongoCollection.DeleteOne(selectorExpression);
				}
			}
		}

		public override void Dispose()
		{

		}
	}
}
