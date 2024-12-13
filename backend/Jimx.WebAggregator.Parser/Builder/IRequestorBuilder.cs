using Jimx.WebAggregator.Builder;
using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Builder
{
	public interface IRequestorBuilder<TOutput> : IBuilder<TOutput>
	{
		Requestor Requestor { get; }
	}
}
