using System.Collections;

namespace Jimx.WebAggregator.Builder
{
	public abstract class Builder<TOutput> : IBuilder<TOutput>
	{
		public ValueFactory<TOutput> ValueFactory { get; }

		internal protected Builder(Func<TOutput> valueFactory)
			:this(new ValueFactory<TOutput>(valueFactory))
		{
			
		}

		internal protected Builder(ValueFactory<TOutput> valueFactory)
		{
			if (valueFactory == null)
				throw new ArgumentNullException(nameof(valueFactory));

			if (valueFactory.IsCalcualated)
				throw new ArgumentException("Value cannot be already calculated", nameof(valueFactory));

			ValueFactory = valueFactory;
		}

		public TOutput Execute()
		{
			return ValueFactory.Value;
		}

		public abstract Builder<TOutputOutput> Wrap<TOutputOutput>(Func<TOutput, TOutputOutput> wrappingValueFactoryFunction);
	}
}