namespace Jimx.WebAggregator.Builder.Helpers
{
	public static class ExtendableHelper
	{
		public static IBuilder<TOutput> ExtendByRequest<TInput, TOutput>(
			this IBuilder<TInput> builder, Func<TInput, TOutput> extendFunc)
		{
			return new SimpleBuilder<TOutput>(
				new Lazy<TOutput>(() => extendFunc(builder.ExecutingFactory.Value)));
		}

		public static IBuilder<IEnumerable<TOutput>> ExtendByRequest<TInput, TOutput>(
			this IBuilder<IEnumerable<TInput>> builder, Func<TInput, TOutput> extendFunc)
		{
			return new SimpleBuilder<IEnumerable<TOutput>>(
				new Lazy<IEnumerable<TOutput>>(() => builder.ExecutingFactory.Value.Select(v => extendFunc(v))));
		}
	}
}
