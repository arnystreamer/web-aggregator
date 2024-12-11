namespace Jimx.WebAggregator.Parser.Http
{
	public class Connection
	{
		public Uri BaseUri { get; set; }
		public HttpHeaders DefaultHeaders { get; }
		public int? RequestsCountPerMinute { get; }

		public Connection(Uri baseUri, HttpHeaders defaultHeaders, int? requestsCountPerMinute)
		{
			if (!baseUri.IsAbsoluteUri)
			{
				throw new ArgumentException(nameof(baseUri), "Must be absolute");
			}

			if (requestsCountPerMinute.HasValue && requestsCountPerMinute.Value <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(requestsCountPerMinute), "Must be more than 0");
			}

			BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));


			DefaultHeaders = defaultHeaders;
			RequestsCountPerMinute = requestsCountPerMinute;
		}

		public Connection(string baseUri, HttpHeaders defaultHeaders, int? requestsCountPerMinute)
			: this(new Uri(baseUri), defaultHeaders, requestsCountPerMinute)
		{

		}

		public Requestor GetRequestor()
		{
			var requestor = new Requestor();
			requestor.RegisterConnection(this);
			return requestor;
		}
	}


}
