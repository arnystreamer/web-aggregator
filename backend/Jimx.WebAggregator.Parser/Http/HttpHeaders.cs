namespace Jimx.WebAggregator.Parser.Http
{
	public class HttpHeaders
	{
		public HttpHeaderItem[] HeaderItems { get; protected set; }

		public HttpHeaders() 
		{
			HeaderItems = Array.Empty<HttpHeaderItem>();
		}

		public HttpHeaders(HttpHeaderItem[] headerItems)
		{
			HeaderItems = headerItems;
		}

		public HttpHeaders((string Header, string? Value)[] items) 
			:this(items.Select(i => new HttpHeaderItem(i.Header, i.Value)).ToArray())
		{

		}
	}
}
