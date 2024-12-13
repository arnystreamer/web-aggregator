
namespace Jimx.WebAggregator.Builder
{
	public class SimpleBuilder<TOutputItem> : IBuilder<TOutputItem>
	{
		public Lazy<TOutputItem> ExecutingFactory { get; }

		internal protected SimpleBuilder(Lazy<TOutputItem> executingFactory)
		{
			ExecutingFactory = executingFactory;
		}

		public virtual TOutputItem Execute()
		{
			return ExecutingFactory.Value;
		}
	}
}
