using MongoDB.Bson;
using MongoDB.Driver;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations
{
	public class UpsertMongoUnitOfWork<TCollectionItem, TIdentity> : MongoUnitOfWork<TCollectionItem>
		where TCollectionItem : IMongoEntity
	{
		private readonly TCollectionItem[] _updatedCollection;
		private readonly UpsertOptions<TCollectionItem, TIdentity> _upsertOptions;

		public UpsertMongoUnitOfWork(TCollectionItem[] updatedCollection, UpsertOptions<TCollectionItem, TIdentity> upsertOptions)
		{
			_updatedCollection = updatedCollection;
			_upsertOptions = upsertOptions;
		}

		public override IEnumerable<TCollectionItem> Do(IMongoCollection<TCollectionItem> mongoCollection)
		{
			var existingItems = mongoCollection.Find(i => true).ToList();

			var replacements = _updatedCollection.Intersect(existingItems, _upsertOptions.IdentityComparer);
			foreach (var replacement in replacements)
			{
				var existingItem = existingItems.Single(i => _upsertOptions.IdentityComparer.Equals(i, replacement));

				var isExistingItemExpired = (_upsertOptions.ActualityComparer?.Compare(existingItem, replacement) ?? -1) < 0;

				if (isExistingItemExpired)
				{
					var selectorExpression = _upsertOptions.IdentityComparerExpression(_upsertOptions.IdentitySelector(replacement));
					replacement.ObjectId = existingItem.ObjectId;
					mongoCollection.ReplaceOne(selectorExpression, replacement);
				}
			}

			var redundands = existingItems.Except(_updatedCollection, _upsertOptions.IdentityComparer);
			foreach (var redundand in redundands)
			{
				var selectorExpression = _upsertOptions.IdentityComparerExpression(_upsertOptions.IdentitySelector(redundand));
				mongoCollection.DeleteOne(selectorExpression);
			}

			var additions = _updatedCollection.Except(existingItems, _upsertOptions.IdentityComparer);
			foreach (var addition in additions)
			{
				mongoCollection.InsertOne(addition);
			}

			return additions.Union(replacements);
		}

		public override void Dispose()
		{

		}
	}
}
