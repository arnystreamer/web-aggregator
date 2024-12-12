namespace Jimx.WebAggregator.Parser.Constructor
{
	public interface IExtensionRequest<TInput, TOutput>
	{
		Task<TOutput> Request(TInput input);
	}
}
