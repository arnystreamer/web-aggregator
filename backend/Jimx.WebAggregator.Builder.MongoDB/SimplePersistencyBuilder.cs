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

		internal protected SimplePersistencyBuilder(MongoConnection mongoConnection, Lazy<TOutputItem> executingFactory)
			: base(executingFactory)
		{
			MongoConnection = mongoConnection;
		}

		public override TOutputItem Execute()
		{
			return ExecutingFactory.Value;
		}

		public override IBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutputItem, TOutputOutput> newExecutingFactoryFunc)
		{
			return ((IPersistencyBuilder<TOutputItem>)this).Wrap(newExecutingFactoryFunc);
		}

		IPersistencyBuilder<TOutputOutput> IPersistencyBuilder<TOutputItem>.Wrap<TOutputOutput>(Func<TOutputItem, TOutputOutput> newExecutingFactoryFunc)
		{
			if (MongoConnection == null)
			{
				throw new InvalidOperationException("MongoConnection figured out to be null");
			}

			return new SimplePersistencyBuilder<TOutputOutput>(MongoConnection,
				new Lazy<TOutputOutput>(() => {
					return newExecutingFactoryFunc(ExecutingFactory.Value);
				}));
		}
	}
}
