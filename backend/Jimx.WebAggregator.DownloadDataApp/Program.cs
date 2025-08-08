using Jimx.WebAggregator.Parser;
using Jimx.WebAggregator.DownloadDataApp;

await new Parser().DoJobAsync(new NumbeoParsingJob());
