using Jimx.WebAggregator.Domain.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations;

public class UpsertMongoUnitOfWork<TCollectionItem, TIdentity> : MongoUnitOfWork<TCollectionItem>
	where TCollectionItem : class, IMongoEntity
{
	private readonly IEnumerable<TCollectionItem> _updatedItems;
	private readonly UpsertOptions<TCollectionItem, TIdentity> _upsertOptions;

	public UpsertMongoUnitOfWork(IEnumerable<TCollectionItem> updatedItems, UpsertOptions<TCollectionItem, TIdentity> upsertOptions)
	{
		_updatedItems = updatedItems;
		_upsertOptions = upsertOptions;
	}

	public override async Task<IEnumerable<TCollectionItem>> DoAsync(ILogger logger, IMongoCollection<TCollectionItem> mongoCollection)
	{
		var existingItems = mongoCollection.Find(i => true).ToList();
		var touchedItemIndices = new List<int>();

		IList<(Task Task, TCollectionItem? Item)> tasks = new List<(Task, TCollectionItem?)>();

		foreach (var updatedItem in _updatedItems)
		{
			var existingItem = existingItems.SingleOrDefault(i => _upsertOptions.IdentityComparer.Equals(i, updatedItem));

			if (existingItem == null)
			{
				logger.LogInformation("{CollectionNamespaceFullName} MongoDb collection: inserting new item", 
					mongoCollection.CollectionNamespace.FullName);
				tasks.Add((mongoCollection.InsertOneAsync(updatedItem), updatedItem));
			}
			else
			{
				touchedItemIndices.Add(existingItems.IndexOf(existingItem));

				var isExistingItemExpired = (_upsertOptions.ActualityComparer?.Compare(existingItem, updatedItem) ?? -1) < 0;

				if (!isExistingItemExpired)
				{
					continue;
				}
					
				var selectorExpression = _upsertOptions.IdentityComparerExpression(_upsertOptions.IdentitySelector(updatedItem));
				updatedItem.ObjectId = existingItem.ObjectId;

				logger.LogInformation("{CollectionNamespaceFullName} MongoDb collection: replacing existing item {ExistingItemObjectId}", 
					mongoCollection.CollectionNamespace.FullName, existingItem.ObjectId);
				tasks.Add((mongoCollection.ReplaceOneAsync(selectorExpression, updatedItem), updatedItem));
			}
		}

		for (var i = 0; i < existingItems.Count; i++)
		{
			if (touchedItemIndices.Contains(i) || _upsertOptions.DoNotDeleteExisting)
			{
				continue;
			}
				
			var selectorExpression = _upsertOptions.IdentityComparerExpression(_upsertOptions.IdentitySelector(existingItems[i]));

			logger.LogInformation("{CollectionNamespaceFullName} MongoDb collection: deleting item {ObjectId}", 
				mongoCollection.CollectionNamespace.FullName, existingItems[i].ObjectId);
			tasks.Add((mongoCollection.DeleteOneAsync(selectorExpression), null));
		}

		await Task.WhenAll(tasks.Select(t => t.Task));
		return tasks.Where(t => t.Item != null).Select(t => t.Item!);
	}

	public override void Dispose()
	{

	}
}