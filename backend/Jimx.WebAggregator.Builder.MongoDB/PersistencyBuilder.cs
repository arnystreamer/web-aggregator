using Jimx.WebAggregator.Persistent.MongoDB;

namespace Jimx.WebAggregator.Builder.MongoDB
{
	public class PersistencyBuilder<TOutputItem> : Builder<TOutputItem>
	{
		public MongoConnection MongoConnection { get; }

		internal protected PersistencyBuilder(MongoConnection mongoConnection, IBuilder<TOutputItem> builder)
			: this(mongoConnection, builder.ValueFactory)
		{

		}

		internal protected PersistencyBuilder(MongoConnection mongoConnection, ValueFactory<TOutputItem> valueFactory)
			: base(valueFactory)
		{
			MongoConnection = mongoConnection;
		}

		public override PersistencyBuilder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutputItem, TOutputOutput> wrappingValueFactoryFunction)
		{
			return new PersistencyBuilder<TOutputOutput>(MongoConnection, ValueFactory.Wrap(wrappingValueFactoryFunction));
		}
	}
}
