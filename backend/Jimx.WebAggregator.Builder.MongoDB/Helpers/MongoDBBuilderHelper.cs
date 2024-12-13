using Jimx.WebAggregator.Persistent.MongoDB;

namespace Jimx.WebAggregator.Builder.MongoDB.Helpers
{
	public static class MongoDBBuilderHelper
	{
		public static IPersistencyBuilder<IEnumerable<TItem>> AdjustMongoDBConnection<TItem>(this IBuilder<IEnumerable<TItem>> collectionBuilder, MongoOptions mongoOptions)
		{
			var mongoConnection = new MongoConnection(mongoOptions);

			return new SimplePersistencyBuilder<IEnumerable<TItem>>(mongoConnection, collectionBuilder);
		}

		public static IPersistencyBuilder<IEnumerable<TItem>> UpsertInMongoDb<TItem, TItemIdentity>(
			this IPersistencyBuilder<IEnumerable<TItem>> collectionBuilder, UpsertOptions<TItem, TItemIdentity> upsertOptions)
			where TItem : IMongoEntity
		{
			return new SimplePersistencyBuilder<IEnumerable<TItem>>(
				collectionBuilder.MongoConnection,
				new Lazy<IEnumerable<TItem>>(
					() =>
					{
						TItem[] items = collectionBuilder.ExecutingFactory.Value.ToArray();
						collectionBuilder.MongoConnection.DoWork<UpsertMongoUnitOfWork<TItem, TItemIdentity>, TItem>(
							new UpsertMongoUnitOfWork<TItem, TItemIdentity>(
								items,
								upsertOptions));
						return items;
					}
				));
		}
	}
}
