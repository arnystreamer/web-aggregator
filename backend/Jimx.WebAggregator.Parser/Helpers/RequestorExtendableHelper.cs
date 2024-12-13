using Jimx.WebAggregator.Parser.Builder;

namespace Jimx.WebAggregator.Parser.Helpers
{
	public static class RequestorExtendableHelper
	{
		private static IRequestorBuilder<TOutput> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			IRequestorBuilder<TInput> builder, TExtensionRequest populationRequest)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>
		{
			return builder.Wrap((value) => populationRequest.Request(value).Result);
		}

		private static IRequestorBuilder<IEnumerable<TOutput>> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			IRequestorBuilder<IEnumerable<TInput>> builder, TExtensionRequest populationRequest)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>
		{
			return builder.Wrap((values) => values.Select(value => populationRequest.Request(value).Result));
		}

		public static IRequestorBuilder<TOutput> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
	this IRequestorBuilder<TInput> builder)
		where TExtensionRequest : ExtensionRequest<TInput, TOutput>, new()
		{
			var extensionRequest = new TExtensionRequest();
			extensionRequest.SetRequestor(builder.Requestor);

			return ExtendByRequest<TExtensionRequest, TInput, TOutput>(builder, extensionRequest);
		}

		public static IRequestorBuilder<IEnumerable<TOutput>> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this IRequestorBuilder<IEnumerable<TInput>> builder)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>, new()
		{
			var extensionRequest = new TExtensionRequest();
			extensionRequest.SetRequestor(builder.Requestor);

			return ExtendByRequest<TExtensionRequest, TInput, TOutput>(builder, extensionRequest);
		}
	}
}
