using Jimx.Common.Comparers;
using System.Linq.Expressions;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations
{
	public class InsertOptions<TItem, TIdentity>
	{
		public Expression<Func<TItem, TIdentity>> IdentitySelectorExpression { get; }
		public Func<TItem, TIdentity> IdentitySelector { get; }
		public Func<TIdentity, Expression<Func<TItem, bool>>> IdentityComparerExpression { get; }
		public bool FailOnExisting { get; }
		public IEqualityComparer<TItem> IdentityComparer { get; }

		public InsertOptions(Expression<Func<TItem, TIdentity>> identitySelectorExpression,
			Func<TIdentity, Expression<Func<TItem, bool>>> identityComparerExpression,
			bool failOnExisting)
		{
			IdentitySelectorExpression = identitySelectorExpression;
			IdentitySelector = identitySelectorExpression.Compile();
			IdentityComparerExpression = identityComparerExpression;
			IdentityComparer = new IdentitySelectorEqualityComparer<TItem, TIdentity>(IdentitySelector);
			FailOnExisting = failOnExisting;
		}
	}
}
