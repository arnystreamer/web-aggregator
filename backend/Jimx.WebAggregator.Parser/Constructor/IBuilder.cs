using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Constructor
{
	public interface IBuilder<TOutput>
	{
		Requestor Requestor { get; }
		Lazy<TOutput> ExecutingFactory { get; }
		TOutput Execute();
	}
}
