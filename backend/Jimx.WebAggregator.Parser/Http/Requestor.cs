using Jimx.Common.Helpers.Strings;
using Jimx.Common.Serializing.Converters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jimx.WebAggregator.Parser.Http;

public class Requestor
{
	private readonly List<ConnectionRequestor> _connectionRequestors = new();
	private readonly ILogger _logger;

	private int _counter;

	public Requestor(ILogger logger)
	{
		_logger = logger;
	}

	public ConnectionRequestor RegisterConnection(Connection connection)
	{
		var connectionRequestor = _connectionRequestors.FirstOrDefault(c => connection.BaseUri.IsUrlSubstringOf(c.BaseUri));

		if (connectionRequestor == null)
		{
			connectionRequestor = new ConnectionRequestor(connection);
			_connectionRequestors.Add(connectionRequestor);
		}

		_logger.LogInformation("Registered connection to {ConnectionBaseUri}", 
			connection.BaseUri);
		return connectionRequestor;
	}

	public async Task<HttpResponseMessage> RequestAsMessage(Uri uri, HttpMethod httpMethod, HttpHeaders? additionalHeaders)
	{
		var connection = _connectionRequestors.Single(c => uri.IsUrlSubstringOf(c.BaseUri));

		HttpResponseMessage response;
		try
		{
			Interlocked.Increment(ref _counter);
			_logger.LogInformation("Requestor [{Counter}]: Enqueuing request to {S}", 
				_counter, uri.ToString());
			response = await connection.EnqueueRequestAsync(uri, httpMethod, additionalHeaders);
			_logger.LogInformation("Requestor [{Counter}]: Request received from {S}", 
				_counter, uri.ToString());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in RequestAsMessage");
			throw;
		}

		response.EnsureSuccessStatusCode();
		return response;
	}

	public async Task<string> RequestAsString(Uri uri, HttpMethod httpMethod, HttpHeaders? additionalHeaders)
	{
		var response = await RequestAsMessage(uri, httpMethod, additionalHeaders);
		return await response.Content.ReadAsStringAsync();
	}

	public async Task<T> RequestAsJson<T>(Uri uri, HttpMethod httpMethod, HttpHeaders? additionalHeaders)
	{
		var responseString = await RequestAsString(uri, httpMethod, additionalHeaders);

		T? responseObject;
		try
		{
			responseObject = JsonConvert.DeserializeObject<T>(responseString, new CustomIntJsonConverter());
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in RequestAsJson");
			throw;
		}

		if (responseObject == null)
		{
			throw new Exception("responseObject is null");
		}

		return responseObject;
	}
}