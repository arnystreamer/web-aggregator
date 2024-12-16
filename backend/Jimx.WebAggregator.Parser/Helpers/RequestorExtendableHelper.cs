using Jimx.WebAggregator.Parser.Builder;

namespace Jimx.WebAggregator.Parser.Helpers
{
	public static class RequestorExtendableHelper
	{
		public static IRequestorBuilder<TOutput> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this IRequestorBuilder<TInput> builder)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>, new()
		{
			var extensionRequest = new TExtensionRequest();
			extensionRequest.SetRequestor(builder.Requestor);

			return builder.Wrap((value) => 
			{
				var result = extensionRequest.Request(value).Result;
				
				return result;
			});
		}

		public static IRequestorBuilder<IEnumerable<TOutput>> ExtendByRequest<TExtensionRequest, TInput, TOutput>(
			this IRequestorBuilder<IEnumerable<TInput>> builder)
				where TExtensionRequest : ExtensionRequest<TInput, TOutput>, new()
		{
			var extensionRequest = new TExtensionRequest();
			extensionRequest.SetRequestor(builder.Requestor);

			return builder.Wrap((values) =>
			{
				var itemsResult = values.Select(value =>
				{
					var result = extensionRequest.Request(value).Result;

					return result;
				}).ToArray();

				return itemsResult.AsEnumerable();
			});
		}
	}
}
