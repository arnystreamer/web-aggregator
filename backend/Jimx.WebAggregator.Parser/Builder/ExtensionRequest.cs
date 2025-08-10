using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Builder;

public abstract class ExtensionRequest<TInput, TOutput> : IExtensionRequest<TInput, TOutput>
{
	private Requestor? _requestor;

	public void SetRequestor(Requestor requestor)
	{
		_requestor = requestor;
	}

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

	protected abstract Uri GetUri(TInput input);
	protected abstract HttpHeaders ProvideHeaders(TInput input);
	protected abstract Task<TOutput> GetInformationFromResponse(TInput input, HttpResponseMessage message);
}