using Jimx.WebAggregator.Builder;
using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Builder
{
	public class RequestorBuilder<TOutputItem> : Builder<TOutputItem>
	{
		public Requestor Requestor { get; }

		internal protected RequestorBuilder(Requestor requestor, IBuilder<TOutputItem> builder)
			:this(requestor, builder.ValueFactory)
		{

		}

		internal protected RequestorBuilder(Requestor requestor, ValueFactory<TOutputItem> valueFactory)
			:base(valueFactory)
		{
			Requestor = requestor;
		}

		public override RequestorBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutputItem, TOutputOutput> wrappingValueFactoryFunction)
		{
			return new RequestorBuilder<TOutputOutput>(Requestor, ValueFactory.Wrap(wrappingValueFactoryFunction));
		}
	}
}
