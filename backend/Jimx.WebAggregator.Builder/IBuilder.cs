namespace Jimx.WebAggregator.Builder
{
	public interface IBuilder<TOutput>
	{
		Lazy<TOutput> ExecutingFactory { get; }
		TOutput Execute();
		IBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutput, TOutputOutput> newExecutingFactoryFunc);
	}
}
