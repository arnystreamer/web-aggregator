namespace Jimx.WebAggregator.Builder.Helpers
{
	public static class MultiplicableHelper
	{
		public static IBuilder<IEnumerable<TOutputItem>> MultiplyByRequest<TInput, TOutputItem>(this IBuilder<TInput> builder, Func<IEnumerable<TOutputItem>> itemsFunc)
		{
			return new SimpleBuilder<IEnumerable<TOutputItem>>(new Lazy<IEnumerable<TOutputItem>>(() => itemsFunc()));
		}
	}
}
