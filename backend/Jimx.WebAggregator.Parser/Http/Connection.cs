using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.Parser.Http;

public class Connection
{
	public ILogger Logger { get; }
	public Uri BaseUri { get; set; }
	public HttpHeaders DefaultHeaders { get; }
	public int? RequestsCountPerMinute { get; }

	public Connection(ILogger logger, Uri baseUri, HttpHeaders defaultHeaders, int? requestsCountPerMinute)
	{
		if (!baseUri.IsAbsoluteUri)
		{
			throw new ArgumentException("Must be absolute", nameof(baseUri));
		}

		if (requestsCountPerMinute.HasValue && requestsCountPerMinute.Value <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(requestsCountPerMinute), "Must be more than 0");
		}

		Logger = logger;
		BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));


		DefaultHeaders = defaultHeaders;
		RequestsCountPerMinute = requestsCountPerMinute;
	}

	public Connection(ILogger logger, string baseUri, HttpHeaders defaultHeaders, int? requestsCountPerMinute)
		: this(logger, new Uri(baseUri), defaultHeaders, requestsCountPerMinute)
	{

	}

	public Requestor GetRequestor()
	{
		var requestor = new Requestor(Logger);
		requestor.RegisterConnection(this);
		return requestor;
	}
}