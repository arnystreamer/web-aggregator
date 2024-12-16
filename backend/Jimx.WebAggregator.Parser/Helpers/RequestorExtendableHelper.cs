using Jimx.WebAggregator.Parser.Builder;

namespace Jimx.WebAggregator.Parser.Helpers
{
	public static class RequestorExtendableHelper
	{
		public static RequestorBuilder<TOutput> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this RequestorBuilder<TInput> builder)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>, new()
		{
			var extensionRequest = new TExtensionRequest();
			extensionRequest.SetRequestor(builder.Requestor);

			return builder.Wrap(value => extensionRequest.Request(value).Result);
		}

		public static RequestorBuilder<IEnumerable<TOutput>> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this RequestorBuilder<IEnumerable<TInput>> builder)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>, new()
		{
			var extensionRequest = new TExtensionRequest();
			extensionRequest.SetRequestor(builder.Requestor);

			return builder.Wrap((values) => values.Select(value => extensionRequest.Request(value).Result));
		}
	}
}
