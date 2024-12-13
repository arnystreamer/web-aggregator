namespace Jimx.WebAggregator.Builder.Helpers
{
	public static class BuilderHelper
	{
		public static IBuilder<IEnumerable<TItem>> Filter<TItem>(this IBuilder<IEnumerable<TItem>> collection, Func<TItem, bool> predicate)
		{
			return new SimpleBuilder<IEnumerable<TItem>>(
				new Lazy<IEnumerable<TItem>>(() => collection.ExecutingFactory.Value.Where(predicate)));
		}

		public static IBuilder<IEnumerable<TItem>> Foreach<TItem>(this IBuilder<IEnumerable<TItem>> collection, Action<TItem> action)
		{
			return new SimpleBuilder<IEnumerable<TItem>>(
				new Lazy<IEnumerable<TItem>>(() => {
					var items = collection.ExecutingFactory.Value;

					foreach (var item in items)
					{
						action(item);
					}

					return items;
				}));
		}
	}
}
