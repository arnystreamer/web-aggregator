using Jimx.Common;
using Jimx.WebAggregator.Parser.Helpers;
using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.Parser.Http
{
	public class ConnectionRequestor : IDisposable
	{
		private readonly ILogger _logger;
		public readonly Uri BaseUri;
		private readonly HttpHeaders _defaultHeaders;
		private readonly int _cooldownInMilliseconds;

		private readonly HttpClient _httpClient = new HttpClient();

		private readonly Queue<QueueItem> _queueItems = new Queue<QueueItem>();

		private readonly Task _workerTask;
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		public ConnectionRequestor(ILogger logger, Uri baseUri, HttpHeaders defaultHeaders, int cooldownInMilliseconds)
		{
			_logger = logger;
			BaseUri = baseUri;
			_defaultHeaders = defaultHeaders;
			_cooldownInMilliseconds = cooldownInMilliseconds;

			_workerTask = Task.Run(Process);
		}

		public ConnectionRequestor(Connection connection)
			: this(connection.Logger, connection.BaseUri, connection.DefaultHeaders, 60000 / (connection.RequestsCountPerMinute ?? 60000))
		{

		}

		public void Dispose()
		{
			_cts.Cancel();
			_workerTask.Wait();
			_queueItems.Clear();
		}

		public async Task<HttpResponseMessage> EnqueueRequestAsync(Uri uri, HttpMethod method, HttpHeaders? overrideHeaders)
		{
			Uri fullUri;
			if (uri.IsAbsoluteUri)
			{
				if (!uri.IsUrlSubstringOf(BaseUri))
				{
					throw new ArgumentException(nameof(uri));
				}

				fullUri = uri;
			}
			else
			{
				fullUri = new Uri(BaseUri, uri);
			}

			HttpHeaders headers = overrideHeaders != null ? _defaultHeaders.Union(overrideHeaders) : _defaultHeaders;

			var message = new HttpRequestMessage(method, fullUri);
			headers.HeaderItems.Foreach(h => message.Headers.TryAddWithoutValidation(h.Header, h.Value));

			TaskCompletionSource<HttpResponseMessage> taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();

			_queueItems.Enqueue(new QueueItem(message, taskCompletionSource));

			_logger.LogInformation($"ConnectionRequestor (count={_queueItems.Count}): Request enqueued {uri.ToString()}");

			return await taskCompletionSource.Task;
		}

		public async Task Process()
		{
			while (!_cts.Token.IsCancellationRequested)
			{
				await Task.Delay(_cooldownInMilliseconds);
				await ProcessItem();
			}
		}

		public async Task ProcessItem()
		{
			if (_queueItems.TryDequeue(out var item))
			{
				try
				{
					var response = await _httpClient.SendAsync(item.Message, _cts.Token);
					item.TaskCompletionSource.SetResult(response);
				}
				catch (Exception ex)
				{
					item.TaskCompletionSource.SetException(ex);
				}
			}
		}

		private record QueueItem(HttpRequestMessage Message, TaskCompletionSource<HttpResponseMessage> TaskCompletionSource);
	}
}
