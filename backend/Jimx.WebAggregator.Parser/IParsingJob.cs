namespace Jimx.WebAggregator.Parser;

public interface IParsingJob
{
	static abstract string ConfigurationName { get; }
	
	Task DoAsync();
}