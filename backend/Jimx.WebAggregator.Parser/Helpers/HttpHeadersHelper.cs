using System.Diagnostics.CodeAnalysis;
using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Helpers
{
	public static class HttpHeadersHelper
	{
		public static HttpHeaders Union(this HttpHeaders httpHeaders, HttpHeaders otherHeaders, bool overwrite = true)
		{
			HttpHeaderItem[] combinedItems;
			if (overwrite)
			{
				combinedItems = otherHeaders.HeaderItems.Union(httpHeaders.HeaderItems).ToArray();
			}
			else
			{
				combinedItems = httpHeaders.HeaderItems.Union(otherHeaders.HeaderItems).ToArray();
			}

			return new HttpHeaders(combinedItems);
		}

		private class HeaderItemByHeaderEqualityComparer : IEqualityComparer<HttpHeaderItem>
		{
			public bool Equals(HttpHeaderItem? x, HttpHeaderItem? y)
			{
				if (x == null || y == null)
				{
					return false;
				}

				return x.Header.Equals(y.Header);
			}

			public int GetHashCode([DisallowNull] HttpHeaderItem obj)
			{
				return obj.Header.GetHashCode();
			}
		}
	}
}
