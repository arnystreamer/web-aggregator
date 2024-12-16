namespace Jimx.WebAggregator.Builder
{
	public interface IBuilder<TOutput>
	{
		Func<TOutput> ValueFactory { get; }
		TOutput Execute();
		IBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutput, TOutputOutput> newExecutingFactoryFunc);
	}
}
