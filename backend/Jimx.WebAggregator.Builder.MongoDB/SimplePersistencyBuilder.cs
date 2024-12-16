using Jimx.WebAggregator.Persistent.MongoDB;

namespace Jimx.WebAggregator.Builder.MongoDB
{
	public class SimplePersistencyBuilder<TOutputItem> : SimpleBuilder<TOutputItem>, IPersistencyBuilder<TOutputItem>
	{
		public MongoConnection MongoConnection { get; }

		internal protected SimplePersistencyBuilder(MongoConnection mongoConnection, IBuilder<TOutputItem> simpleBuilder)
			: this(mongoConnection, simpleBuilder.ValueFactory)
		{

		}

		internal protected SimplePersistencyBuilder(MongoConnection mongoConnection, Func<TOutputItem> valueFactory)
			: base(valueFactory)
		{
			MongoConnection = mongoConnection;
		}

		public override IBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutputItem, TOutputOutput> newExecutingFactoryFunc)
		{
			return ((IPersistencyBuilder<TOutputItem>)this).Wrap(newExecutingFactoryFunc);
		}

		IPersistencyBuilder<TOutputOutput> IPersistencyBuilder<TOutputItem>.Wrap<TOutputOutput>(Func<TOutputItem, TOutputOutput> newValueFactoryFunc)
		{
			if (MongoConnection == null)
			{
				throw new InvalidOperationException("MongoConnection figured out to be null");
			}

			return new SimplePersistencyBuilder<TOutputOutput>(
				MongoConnection,
				() => newValueFactoryFunc(ValueFactory()));
		}
	}
}
