namespace Jimx.WebAggregator.Builder
{
	public interface IBuilder<TOutput>
	{
		Lazy<TOutput> ExecutingFactory { get; }
		TOutput Execute();
	}
}
