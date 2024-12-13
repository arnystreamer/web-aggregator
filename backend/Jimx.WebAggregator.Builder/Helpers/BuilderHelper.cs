using Jimx.Common;

namespace Jimx.WebAggregator.Builder.Helpers
{
	public static class BuilderHelper
	{
		public static IBuilder<IEnumerable<TItem>> Filter<TItem>(this IBuilder<IEnumerable<TItem>> builder, Func<TItem, bool> predicate)
		{
			return builder.Wrap(v => v.Where(predicate));
		}

		public static IBuilder<IEnumerable<TItem>> Foreach<TItem>(this IBuilder<IEnumerable<TItem>> builder, Action<TItem> action)
		{
			return builder.Wrap(v => v.ForeachAndReturn(action));
		}

		public static IBuilder<IEnumerable<TOutput>> Map<TInput, TOutput>(this IBuilder<IEnumerable<TInput>> builder, Func<TInput, TOutput> mapperFunc)
		{
			return builder.Wrap(v => v.Select(mapperFunc));
		}

		public static IBuilder<TOutput> Map<TInput, TOutput>(this IBuilder<TInput> builder, Func<TInput, TOutput> mapperFunc)
		{
			return builder.Wrap(mapperFunc);
		}
	}
}
