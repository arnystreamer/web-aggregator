using Jimx.WebAggregator.Parser.Builder;
using Jimx.WebAggregator.Parser.Http;
using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.Parser.Helpers
{
	public static class RequestorMultiplicableHelper
	{
		public static IRequestorBuilder<IEnumerable<TOutputItem>> MultiplyWithStaticList<TInput, TOutputItem>(this IRequestorBuilder<TInput> builder, IEnumerable<TOutputItem> items)
		{
			return builder.Wrap(_ => 
			{
				return items; 
			});
		}

		public static IRequestorBuilder<IEnumerable<TOutputItem>> MultiplyWithStaticList<TOutputItem>(this Connection connection, IEnumerable<TOutputItem> items)
		{
			return new SimpleRequestorBuilder<IEnumerable<TOutputItem>>(
				connection.GetRequestor(),
				() => 
				{
					return items; 
				});
		}
	}
}
