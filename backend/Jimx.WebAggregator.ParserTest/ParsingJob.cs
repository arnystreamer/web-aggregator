using Jimx.WebAggregator.Parser;

namespace Jimx.WebAggregator.ParserTest
{
	internal class ParsingJob : IParsingJob
	{
		public Task DoAsync()
		{
			return Task.CompletedTask;
		}
	}
}
