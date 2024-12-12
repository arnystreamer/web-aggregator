using Jimx.WebAggregator.Parser.Constructor;

namespace Jimx.WebAggregator.Parser.Helpers
{
    public static class ExtendableHelper
	{
		public static IBuilder<TOutput> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this IBuilder<TInput> populatable, TExtensionRequest populationRequest)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>
		{
			populationRequest.SetRequestor(populatable.Requestor);

			return new SimpleBuilder<TOutput>(
				populatable.Requestor,
				new Lazy<TOutput>(() => populationRequest.Request(populatable.ExecutingFactory.Value).Result));
		}

		public static IBuilder<IEnumerable<TOutput>> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this IBuilder<IEnumerable<TInput>> populatable, TExtensionRequest populationRequest)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>
		{
			populationRequest.SetRequestor(populatable.Requestor);

			return new SimpleBuilder<IEnumerable<TOutput>>(
				populatable.Requestor,
				new Lazy<IEnumerable<TOutput>>(() => populatable.ExecutingFactory.Value.Select(v => populationRequest.Request(v).Result)));
		}
	}
}
