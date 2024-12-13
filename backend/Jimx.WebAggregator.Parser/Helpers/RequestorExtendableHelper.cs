using Jimx.WebAggregator.Builder.Helpers;
using Jimx.WebAggregator.Parser.Builder;

namespace Jimx.WebAggregator.Parser.Helpers
{
    public static class RequestorExtendableHelper
	{
		public static IRequestorBuilder<TOutput> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this IRequestorBuilder<TInput> populatable, TExtensionRequest populationRequest)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>
		{
			populationRequest.SetRequestor(populatable.Requestor);

			var simpleBuilder = ExtendableHelper.ExtendByRequest(populatable, (value) => populationRequest.Request(value).Result);
			return new SimpleRequestorBuilder<TOutput>(populatable.Requestor, simpleBuilder);
		}

		public static IRequestorBuilder<IEnumerable<TOutput>> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this IRequestorBuilder<IEnumerable<TInput>> populatable, TExtensionRequest populationRequest)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>
		{
			populationRequest.SetRequestor(populatable.Requestor);

			var simpleBuilder = ExtendableHelper.ExtendByRequest(populatable, (IEnumerable<TInput> values) => values.Select(value => populationRequest.Request(value).Result));

			return new SimpleRequestorBuilder<IEnumerable<TOutput>>(populatable.Requestor, simpleBuilder);
		}
	}
}
