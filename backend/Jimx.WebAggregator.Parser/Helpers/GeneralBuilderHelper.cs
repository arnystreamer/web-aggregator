using Jimx.WebAggregator.Parser.Constructor;
using System.Linq;

namespace Jimx.WebAggregator.Parser.Helpers
{
	public static class GeneralBuilderHelper
	{
		public static IBuilder<IEnumerable<TItem>> Filter<TItem>(this IBuilder<IEnumerable<TItem>> collection, Func<TItem, bool> predicate)
		{
			return new SimpleBuilder<IEnumerable<TItem>>(
				collection.Requestor,
				new Lazy<IEnumerable<TItem>>(() => collection.ExecutingFactory.Value.Where(predicate)));
		}

		public static IBuilder<IEnumerable<TItem>> Foreach<TItem>(this IBuilder<IEnumerable<TItem>> collection, Action<TItem> action)
		{
			return new SimpleBuilder<IEnumerable<TItem>>(
				collection.Requestor,
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
