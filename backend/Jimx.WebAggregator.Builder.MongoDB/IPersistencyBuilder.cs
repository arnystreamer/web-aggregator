using Jimx.WebAggregator.Persistent.MongoDB;

namespace Jimx.WebAggregator.Builder.MongoDB
{
	public interface IPersistencyBuilder<TOutput> : IBuilder<TOutput>
	{
		MongoConnection MongoConnection { get; }
	}
}
