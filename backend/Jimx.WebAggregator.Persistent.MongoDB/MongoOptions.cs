namespace Jimx.WebAggregator.Persistent.MongoDB
{
	public record MongoOptions(string ConnectionString, string DatabaseName, string CollectionName);
}
