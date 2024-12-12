using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Constructor
{
	public abstract class ExtensionRequest<TInput, TOutput> : IExtensionRequest<TInput, TOutput>
	{
		private Requestor? _requestor = null;

		public virtual void SetRequestor(Requestor requestor)
		{
			_requestor = requestor;
		}

		public abstract Uri GetUri(TInput input);

		public virtual async Task<TOutput> Request(TInput input)
		{
			if (_requestor == null)
			{
				throw new InvalidOperationException();
			}

			var uri = GetUri(input);
			var message = await _requestor.RequestAsMessage(uri, HttpMethod.Get, new HttpHeaders());
			return await GetInformationFromResponse(input, message);
		}

		public abstract Task<TOutput> GetInformationFromResponse(TInput input, HttpResponseMessage message);
	}
}
