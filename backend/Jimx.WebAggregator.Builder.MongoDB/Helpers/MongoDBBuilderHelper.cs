using Jimx.WebAggregator.Builder.MongoDB.Models;
using Jimx.WebAggregator.Domain.MongoDB;
using Jimx.WebAggregator.Persistent.MongoDB;
using Jimx.WebAggregator.Persistent.MongoDB.Operations;

namespace Jimx.WebAggregator.Builder.MongoDB.Helpers
{
    public static class MongoDBBuilderHelper
	{
		public static PersistencyBuilder<IEnumerable<TItem>> AdjustMongoDBConnection<TItem>(this IBuilder<IEnumerable<TItem>> collectionBuilder, MongoConnection mongoConnection)
		{
			return new PersistencyBuilder<IEnumerable<TItem>>(
				mongoConnection,
				collectionBuilder);
		}

		public static PersistencyBuilder<IEnumerable<TItem>> AdjustMongoDBConnection<TItem>(this IBuilder<IEnumerable<TItem>> collectionBuilder, MongoOptions mongoOptions)
		{
			var mongoConnection = new MongoConnection(mongoOptions);

			return AdjustMongoDBConnection<TItem>(collectionBuilder, mongoConnection);
		}

		public static PersistencyBuilder<IEnumerable<TEntity>> UpsertInMongoDb<TBase, TEntity, TEntityIdentity>(
			this PersistencyBuilder<IEnumerable<TBase>> collectionBuilder, UpsertOptions<TEntity, TEntityIdentity> upsertOptions, 
			Func<TBase, TEntity> collectionSelector)
			where TEntity : class, IMongoEntity
		{
			return collectionBuilder.Wrap(value =>
			{
				var resolvedValue = value.ToList();
				IEnumerable<TEntity> selectedItems = resolvedValue.Select(v => collectionSelector(v));

				var result = collectionBuilder.MongoConnection.DoWork<UpsertMongoUnitOfWork<TEntity, TEntityIdentity>, TEntity>(
					new UpsertMongoUnitOfWork<TEntity, TEntityIdentity>(selectedItems.ToArray(), upsertOptions));
				return selectedItems;
			});
		}

		public static PersistencyBuilder<DictionaryExtractionResult<TItem, TDictionaryEntity>> ExtractDictionary<TItem, TDictionaryEntity, TDictionaryEntityIdentity>(
			this PersistencyBuilder<IEnumerable<TItem>> collectionBuilder, 
				Func<TItem, IEnumerable<TDictionaryEntity>> dictionaryItemsSelector,
				InsertOptions<TDictionaryEntity, TDictionaryEntityIdentity> insertOptions)
			where TDictionaryEntity : IMongoEntity
		{
			return collectionBuilder.Wrap(value =>
			{
				var resolvedValue = value.ToList();
				var itemsPlain = resolvedValue.SelectMany(v => dictionaryItemsSelector(v)).ToArray();

				var result = collectionBuilder.MongoConnection.DoWork<InsertMongoUnitOfWork<TDictionaryEntity, TDictionaryEntityIdentity>, TDictionaryEntity>(
					new InsertMongoUnitOfWork<TDictionaryEntity, TDictionaryEntityIdentity>(
						itemsPlain,
						insertOptions));

				return new DictionaryExtractionResult<TItem, TDictionaryEntity>(resolvedValue, result.AllItems);
			});
		}
	}
}
