using Jimx.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jimx.WebAggregator.Parser.Http
{
	public class Requestor
	{
		private List<ConnectionRequestor> _connectionRequestors = new List<ConnectionRequestor>();
		private ILogger _logger;

		private int _counter = 0;

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

			_logger.LogInformation($"Registered connection to {connection.BaseUri}");
			return connectionRequestor;
		}

		public async Task<HttpResponseMessage> RequestAsMessage(Uri uri, HttpMethod httpMethod, HttpHeaders? additionalHeaders)
		{
			var connection = _connectionRequestors.Single(c => uri.IsUrlSubstringOf(c.BaseUri));

			HttpResponseMessage response;
			try
			{
				Interlocked.Increment(ref _counter);
				_logger.LogInformation($"Requestor [{_counter}]: Enqueuing request to {uri.ToString()}");
				response = await connection.EnqueueRequestAsync(uri, httpMethod, additionalHeaders);
				_logger.LogInformation($"Requestor [{_counter}]: Request received from {uri.ToString()}");
			}
			catch (Exception ex)
			{
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
				responseObject = JsonConvert.DeserializeObject<T>(responseString, new CustomIntConverter());
			}
			catch (Exception ex)
			{
				throw;
			}

			if (responseObject == null)
			{
				throw new Exception("responseObject is null");
			}

			return responseObject;
		}
	}
}
