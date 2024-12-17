using Jimx.WebAggregator.Builder;
using Jimx.WebAggregator.Parser.Builder;
using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Helpers
{
	public static class RequestorMultiplicableHelper
	{
		public static RequestorBuilder<IEnumerable<TOutputItem>> MultiplyWithStaticList<TOutputItem>(this Connection connection, IEnumerable<TOutputItem> items)
		{
			return new RequestorBuilder<IEnumerable<TOutputItem>>(
				connection.GetRequestor(),
				new ValueFactory<IEnumerable<TOutputItem>>(() => items));
		}
	}
}
