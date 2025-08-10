using Jimx.WebAggregator.Domain.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations;

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

	public override async Task<IEnumerable<TCollectionItem>> DoAsync(ILogger logger, IMongoCollection<TCollectionItem> mongoCollection)
	{
		var existingItems = (await mongoCollection.FindAsync(i => true)).ToList();

		IList<(Task Task,TCollectionItem Item)> tasks = new List<(Task, TCollectionItem)>();

		foreach (var updatedItem in _updatedItems)
		{
			var existingItem = existingItems.SingleOrDefault(i => _insertOptions.IdentityComparer.Equals(i, updatedItem));

			if (existingItem == null)
			{
				logger.LogInformation("{CollectionNamespaceFullName} MongoDb collection: inserting new item", 
					mongoCollection.CollectionNamespace.FullName);
				tasks.Add((mongoCollection.InsertOneAsync(updatedItem), updatedItem));
			}
		}

		await Task.WhenAll(tasks.Select(t => t.Task));
		return tasks.Select(t => t.Item);
	}

	public override void Dispose()
	{

	}
}