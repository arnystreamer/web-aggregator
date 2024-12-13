using Jimx.WebAggregator.Persistent.MongoDB;

namespace Jimx.WebAggregator.Builder.MongoDB
{
	public class SimplePersistencyBuilder<TOutputItem> : SimpleBuilder<TOutputItem>, IPersistencyBuilder<TOutputItem>
	{
		public MongoConnection MongoConnection { get; }

		internal protected SimplePersistencyBuilder(MongoConnection mongoConnection, IBuilder<TOutputItem> simpleBuilder)
			: this(mongoConnection, simpleBuilder.ExecutingFactory)
		{

		}

		internal SimplePersistencyBuilder(MongoConnection mongoConnection, Lazy<TOutputItem> executingFactory)
			: base(executingFactory)
		{
			MongoConnection = mongoConnection;
		}

		public override TOutputItem Execute()
		{
			return ExecutingFactory.Value;
		}
	}
}
