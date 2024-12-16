using Jimx.Common.Comparers;
using System.Linq.Expressions;

namespace Jimx.WebAggregator.Persistent.MongoDB.Operations
{
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
			IdentityComparer = new IdentitySelectorEqualityComparer<TItem, TIdentity>(IdentitySelector);
			ActualityComparer = actualityComparer;
		}		
	}


}
