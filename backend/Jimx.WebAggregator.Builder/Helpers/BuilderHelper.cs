using Jimx.Common;

namespace Jimx.WebAggregator.Builder.Helpers
{
	public static class BuilderHelper
	{
		public static Builder<IEnumerable<TOutputItem>> MultiplyWithStaticList<TInput, TOutputItem>(this Builder<TInput> builder, IEnumerable<TOutputItem> items)
		{
			return builder.Wrap(_ => items);
		}

		public static Builder<IEnumerable<TItem>> Filter<TItem>(this Builder<IEnumerable<TItem>> builder, Func<TItem, bool> predicate)
		{
			return builder.Wrap(v => v.Where(predicate));
		}

		public static Builder<IEnumerable<TItem>> Foreach<TItem>(this Builder<IEnumerable<TItem>> builder, Action<TItem> action)
		{
			return builder.Wrap(v => v.ForeachAndReturn(action));
		}

		public static Builder<IEnumerable<TOutput>> Map<TInput, TOutput>(this Builder<IEnumerable<TInput>> builder, Func<TInput, TOutput> mapperFunc)
		{
			return builder.Wrap(v => v.Select(mapperFunc));
		}

		public static Builder<TOutput> Map<TInput, TOutput>(this Builder<TInput> builder, Func<TInput, TOutput> mapperFunc)
		{
			return builder.Wrap(mapperFunc);
		}
	}
}
