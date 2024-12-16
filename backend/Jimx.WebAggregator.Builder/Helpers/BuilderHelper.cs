using Jimx.Common;

namespace Jimx.WebAggregator.Builder.Helpers
{
	public static class BuilderHelper
	{
		public static IBuilder<IEnumerable<TOutputItem>> MultiplyWithStaticList<TInput, TOutputItem>(this IBuilder<TInput> builder, IEnumerable<TOutputItem> items)
		{
			return builder.Wrap(_ =>
			{
				return items;
			});
		}

		public static IBuilder<IEnumerable<TItem>> Filter<TItem>(this IBuilder<IEnumerable<TItem>> builder, Func<TItem, bool> predicate)
		{
			return builder.Wrap(v => 
			{
				return v.Where(predicate);
			});
		}

		public static IBuilder<IEnumerable<TItem>> Foreach<TItem>(this IBuilder<IEnumerable<TItem>> builder, Action<TItem> action)
		{
			return builder.Wrap(v => 
			{
				return v.ForeachAndReturn(action);
			});
		}

		public static IBuilder<IEnumerable<TOutput>> Map<TInput, TOutput>(this IBuilder<IEnumerable<TInput>> builder, Func<TInput, TOutput> mapperFunc)
		{
			return builder.Wrap(v => 
			{ 
				return v.Select(mapperFunc);
			});
		}

		public static IBuilder<TOutput> Map<TInput, TOutput>(this IBuilder<TInput> builder, Func<TInput, TOutput> mapperFunc)
		{
			return builder.Wrap(v =>
			{
				return mapperFunc(v);
			});
		}
	}
}
