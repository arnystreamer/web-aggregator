
namespace Jimx.WebAggregator.Builder
{
	public abstract class SimpleBuilder<TOutput> : IBuilder<TOutput>
	{
		public Lazy<TOutput> ExecutingFactory { get; }

		internal protected SimpleBuilder(Lazy<TOutput> executingFactory)
		{
			ExecutingFactory = executingFactory;
		}

		public virtual TOutput Execute()
		{
			return ExecutingFactory.Value;
		}

		public abstract IBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutput, TOutputOutput> newExecutingFactoryFunc);
	}
}
