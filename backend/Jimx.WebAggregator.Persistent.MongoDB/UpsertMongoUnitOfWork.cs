using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Jimx.WebAggregator.Persistent.MongoDB
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

		public override void Do(IMongoCollection<TCollectionItem> mongoCollection)
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
		}

		public override void Dispose()
		{

		}
	}

	public class UpsertOptions<TItem, TIdentity>
	{
		public Expression<Func<TItem, TIdentity>> IdentitySelectorExpression { get; }
		public Func<TItem, TIdentity> IdentitySelector { get; }
		public Func<TIdentity, Expression<Func<TItem, bool>>> IdentityComparerExpression { get; }
		public IEqualityComparer<TItem> IdentityComparer { get; }
		public IComparer<TItem>? ActualityComparer { get; } = null;

		public UpsertOptions(Expression<Func<TItem, TIdentity>> identitySelectorExpression,
			Func<TIdentity, Expression<Func<TItem, bool>>> identityComparerExpression,
			IComparer<TItem>? actualityComparer)
		{
			IdentitySelectorExpression = identitySelectorExpression;
			IdentitySelector = identitySelectorExpression.Compile();
			IdentityComparerExpression = identityComparerExpression;
			IdentityComparer = new IdentitySelectorEqualityComparer<TItem>(IdentitySelector);
			ActualityComparer = actualityComparer;
		}

		private class IdentitySelectorEqualityComparer<T> : IEqualityComparer<T>
		{
			private readonly Func<T, TIdentity> _identitySelector;

			public IdentitySelectorEqualityComparer(Func<T, TIdentity> identitySelector)
			{
				_identitySelector = identitySelector ?? throw new ArgumentNullException(nameof(identitySelector));
			}

			public bool Equals(T? x, T? y)
			{
				if (x == null || y == null)
				{
					return false;
				}

				var identityX = _identitySelector(x);
				var identityY = _identitySelector(y);

				if (identityX == null || identityY == null)
				{
					return false;
				}

				return identityX.Equals(identityY);
			}

			public int GetHashCode([DisallowNull] T obj)
			{
				return _identitySelector(obj)?.GetHashCode() ?? 0;
			}
		}
	}
}
