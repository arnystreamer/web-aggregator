namespace Jimx.WebAggregator.Parser.Builder
{
	public interface IExtensionRequest<TInput, TOutput>
	{
		Task<TOutput> Request(TInput input);
	}
}
