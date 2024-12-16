namespace Jimx.WebAggregator.Builder
{
	public abstract class SimpleBuilder<TOutput> : IBuilder<TOutput>
	{
		public Func<TOutput> ValueFactory { get; }

		internal protected SimpleBuilder(Func<TOutput> valueFactory)
		{
			ValueFactory = valueFactory;
		}

		public TOutput Execute()
		{
			return ValueFactory();
		}

		public abstract IBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutput, TOutputOutput> newExecutingFactoryFunc);
	}
}
