using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations
{
	public class InsertMongoUnitOfWork<TCollectionItem, TIdentity> : MongoUnitOfWork<TCollectionItem>
		where TCollectionItem : IMongoEntity
	{
		private readonly TCollectionItem[] _updatedCollection;
		private readonly InsertOptions<TCollectionItem, TIdentity> _insertOptions;

		public InsertMongoUnitOfWork(TCollectionItem[] updatedCollection, InsertOptions<TCollectionItem, TIdentity> insertOptions)
		{
			_updatedCollection = updatedCollection;
			_insertOptions = insertOptions;
		}

		public override IEnumerable<TCollectionItem> Do(IMongoCollection<TCollectionItem> mongoCollection)
		{
			var existingItems = mongoCollection.Find(i => true).ToList();
			var additions = _updatedCollection.Except(existingItems, _insertOptions.IdentityComparer).ToList();

			if (additions.Any())
			{
				mongoCollection.InsertMany(additions);
			}

			return additions;
		}

		public override void Dispose()
		{

		}
	}
}
