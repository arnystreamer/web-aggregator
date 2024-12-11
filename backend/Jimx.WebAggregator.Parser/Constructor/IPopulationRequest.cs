namespace Jimx.WebAggregator.Parser.Constructor
{
	public interface IPopulationRequest<TInput, TOutput>
	{
		Task<TOutput> Request(TInput input);
	}
}
