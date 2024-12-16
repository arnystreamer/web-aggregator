using Jimx.WebAggregator.Builder.MongoDB.Models;
using Jimx.WebAggregator.Persistent.MongoDB;
using Jimx.WebAggregator.Persistent.MongoDB.Operations;

namespace Jimx.WebAggregator.Builder.MongoDB.Helpers
{
    public static class MongoDBBuilderHelper
	{
		public static IPersistencyBuilder<IEnumerable<TItem>> AdjustMongoDBConnection<TItem>(this IBuilder<IEnumerable<TItem>> collectionBuilder, MongoOptions mongoOptions)
		{
			var mongoConnection = new MongoConnection(mongoOptions);

			return new SimplePersistencyBuilder<IEnumerable<TItem>>(
				mongoConnection,
				collectionBuilder);
		}

		public static IPersistencyBuilder<IEnumerable<TEntity>> UpsertInMongoDb<TBase, TEntity, TEntityIdentity>(
			this IPersistencyBuilder<IEnumerable<TBase>> collectionBuilder, UpsertOptions<TEntity, TEntityIdentity> upsertOptions, 
			Func<TBase, TEntity> collectionSelector)
			where TEntity : IMongoEntity
		{
			return collectionBuilder.Wrap(value =>
			{
				IEnumerable<TEntity> selectedItems = value.Select(v => collectionSelector(v));
				var result = collectionBuilder.MongoConnection.DoWork<UpsertMongoUnitOfWork<TEntity, TEntityIdentity>, TEntity>(
					new UpsertMongoUnitOfWork<TEntity, TEntityIdentity>(selectedItems.ToArray(), upsertOptions));

				return selectedItems;
			});
		}

		public static IPersistencyBuilder<DictionaryExtractionResult<TItem, TDictionaryEntity>> ExtractDictionary<TItem, TDictionaryEntity, TDictionaryEntityIdentity>(
			this IPersistencyBuilder<IEnumerable<TItem>> collectionBuilder, 
				Func<TItem, IEnumerable<TDictionaryEntity>> dictionaryItemsSelector,
				InsertOptions<TDictionaryEntity, TDictionaryEntityIdentity> insertOptions)
			where TDictionaryEntity : IMongoEntity
		{
			return collectionBuilder.Wrap(value =>
			{
				TDictionaryEntity[] itemsPlain = value.SelectMany(v => dictionaryItemsSelector(v)).ToArray();
				var result = collectionBuilder.MongoConnection.DoWork<InsertMongoUnitOfWork<TDictionaryEntity, TDictionaryEntityIdentity>, TDictionaryEntity>(
					new InsertMongoUnitOfWork<TDictionaryEntity, TDictionaryEntityIdentity>(
						itemsPlain,
						insertOptions));

				return new DictionaryExtractionResult<TItem, TDictionaryEntity>(value, result.AllItems);
			});
		}
	}
}
