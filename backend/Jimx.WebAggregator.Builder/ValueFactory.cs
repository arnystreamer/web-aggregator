namespace Jimx.WebAggregator.Builder
{
	public class ValueFactory<TOutput>
	{
		private readonly Func<TOutput> _valueFactory;
		private TOutput? _calculatedValue = default;

		public bool IsCalcualated { get; private set; } = false;

		public ValueFactory(Func<TOutput> valueFactory)
		{
			_valueFactory = valueFactory;
		}

		public TOutput Value
		{
			get
			{
				if (!IsCalcualated)
				{
					_calculatedValue = _valueFactory();
					IsCalcualated = true;
				}
				return _calculatedValue!;
			}
		}

		public ValueFactory<TOutputOutput> Wrap<TOutputOutput>(Func<TOutput, TOutputOutput> wrappingValueFactoryFunction)
		{
			return new ValueFactory<TOutputOutput>(() => wrappingValueFactoryFunction(Value));
		}
	}
}