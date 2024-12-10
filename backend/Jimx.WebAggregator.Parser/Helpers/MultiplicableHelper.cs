using Jimx.WebAggregator.Parser.Constructor;
using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Helpers
{
    public static class MultiplicableHelper
	{
		public static IBuilder<IEnumerable<TOutputItem>> MultiplyByRequest<TInput, TOutputItem>(this IBuilder<TInput> multiplicable, Func<Requestor, IEnumerable<TOutputItem>> itemsFunc)
		{
			return new SimpleBuilder<IEnumerable<TOutputItem>>(multiplicable.Requestor,
				new Lazy<IEnumerable<TOutputItem>>(() => itemsFunc(multiplicable.Requestor)));
		}

		public static IBuilder<IEnumerable<TOutputItem>> MultiplyWithStaticList<TInput, TOutputItem>(this IBuilder<TInput> multiplicable, IEnumerable<TOutputItem> items)
		{
			return new SimpleBuilder<IEnumerable<TOutputItem>>(multiplicable.Requestor,
				new Lazy<IEnumerable<TOutputItem>>(() => items));
		}

		public static IBuilder<IEnumerable<TOutputItem>> MultiplyWithStaticList<TOutputItem>(this Connection connection, IEnumerable<TOutputItem> items)
		{
			return new SimpleBuilder<IEnumerable<TOutputItem>>(connection.GetRequestor(),
				new Lazy<IEnumerable<TOutputItem>>(() => items));
		}
	}
}
