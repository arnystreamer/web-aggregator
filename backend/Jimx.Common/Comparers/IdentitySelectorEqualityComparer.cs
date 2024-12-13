using System.Diagnostics.CodeAnalysis;

namespace Jimx.Common.Comparers
{
	public class IdentitySelectorEqualityComparer<T, TIdentity> : IEqualityComparer<T>
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
