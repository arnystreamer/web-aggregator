namespace Jimx.Common
{
	public static class UriHelper
	{
		public static bool IsUrlSubstringOf(this Uri uri, Uri prefix)
		{
			return uri.ToString().ToLowerInvariant().StartsWith(prefix.ToString().ToLowerInvariant());
		}
	}
}
