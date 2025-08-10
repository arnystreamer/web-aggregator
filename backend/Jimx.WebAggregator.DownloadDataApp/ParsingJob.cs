using Jimx.WebAggregator.Parser;

namespace Jimx.WebAggregator.DownloadDataApp;

internal class ParsingJob : IParsingJob
{
	public Task DoAsync()
	{
		return Task.CompletedTask;
	}
}