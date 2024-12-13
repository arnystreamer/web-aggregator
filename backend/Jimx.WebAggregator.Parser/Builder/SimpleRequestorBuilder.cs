using Jimx.WebAggregator.Builder;
using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Builder
{
	public class SimpleRequestorBuilder<TOutputItem> : SimpleBuilder<TOutputItem>, IRequestorBuilder<TOutputItem>
	{
		public Requestor Requestor { get; }

		internal protected SimpleRequestorBuilder(Requestor requestor, IBuilder<TOutputItem> simpleBuilder)
			:this(requestor, simpleBuilder.ExecutingFactory)
		{

		}

		internal SimpleRequestorBuilder(Requestor requestor, Lazy<TOutputItem> executingFactory)
			:base(executingFactory)
		{
			Requestor = requestor;
		}

		public override TOutputItem Execute()
		{
			return ExecutingFactory.Value;
		}
	}
}
