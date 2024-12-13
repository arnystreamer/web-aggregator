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

		internal protected SimpleRequestorBuilder(Requestor requestor, Lazy<TOutputItem> executingFactory)
			:base(executingFactory)
		{
			Requestor = requestor;
		}

		public override TOutputItem Execute()
		{
			return ExecutingFactory.Value;
		}

		public override IBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutputItem, TOutputOutput> newExecutingFactoryFunc)
		{
			return ((IRequestorBuilder<TOutputItem>)this).Wrap(newExecutingFactoryFunc);
		}

		IRequestorBuilder<TOutputOutput> IRequestorBuilder<TOutputItem>.Wrap<TOutputOutput>(Func<TOutputItem, TOutputOutput> newExecutingFactoryFunc)
		{
			if (Requestor == null)
			{
				throw new InvalidOperationException("Requestor figured out to be null");
			}

			return new SimpleRequestorBuilder<TOutputOutput>(Requestor,
				new Lazy<TOutputOutput>(() => {
					return newExecutingFactoryFunc(ExecutingFactory.Value);
				}));
		}
	}
}
