namespace Jimx.Common
{
	public static class EnumerableHelper
	{
		public static void Foreach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (var item in items)
			{
				action(item);
			}
		}
	}
}
