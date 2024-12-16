using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.Persistent.MongoDB
{
	public record MongoOptions(ILogger Logger, string ConnectionString, string DatabaseName, string CollectionName);
}
