namespace Jimx.WebAggregator.Builder
{
	public interface IBuilder<TOutput>
	{
		ValueFactory<TOutput> ValueFactory { get; }
		TOutput? Execute();
	}
}
