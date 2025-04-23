namespace Jimx.WebAggregator.Parser
{
	public class Parser
	{
		public async Task DoJobAsync(IParsingJob job)
		{
			await job.DoAsync();
		}
	}
}
