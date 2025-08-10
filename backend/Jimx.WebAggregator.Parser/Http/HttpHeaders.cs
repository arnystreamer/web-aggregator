using System.Text.RegularExpressions;

namespace Jimx.WebAggregator.Parser.Http;

public class HttpHeaders
{
	public HttpHeaderItem[] HeaderItems { get; protected set; }

	public HttpHeaders() 
	{
		HeaderItems = [];
	}

	public HttpHeaders(HttpHeaderItem[] headerItems)
	{
		HeaderItems = headerItems;
	}

	public HttpHeaders((string Header, string? Value)[] items) 
		:this(items.Select(i => new HttpHeaderItem(i.Header, i.Value)).ToArray())
	{

	}

	public static HttpHeaders CreateFromCurlCommand(string curlBashCommand)
	{
		var matches = Regex.Matches(curlBashCommand.Trim(), @"['].+?[']|[^ \n\r\\]+")
			.Select(v => v.Value)
			.Where(v => !string.IsNullOrWhiteSpace(v))
			.ToArray();

		if (matches[0] != "curl")
			throw new Exception("Command is not curl invocation");

		IList<HttpHeaderItem> headers = [];

		for (var i = 2; i < matches.Length; i++)
		{
			var argument = matches[i];

			if (argument.StartsWith("-") || argument.StartsWith("--"))
			{
				i++;

				if (argument == "-H")
				{
					var headerArgumentValue = matches[i].Trim('\'');

					var headerAndValue = headerArgumentValue.Split(':', 2, StringSplitOptions.TrimEntries);

					headers.Add(new HttpHeaderItem(headerAndValue[0], headerAndValue[1]));
				}
			}
		}

		return new HttpHeaders(headers.ToArray());
	}
}