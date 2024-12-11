using Jimx.Common;

namespace Jimx.WebAggregator.Parser.Http
{
	public class ConnectionRequestor : IDisposable
	{
		public readonly Uri BaseUri;
		private readonly HttpHeaders _defaultHeaders;
		private readonly int _cooldownInMilliseconds;

		private readonly HttpClient _httpClient = new HttpClient();

		private readonly Queue<QueueItem> _queueItems = new Queue<QueueItem>();

		private readonly Task _workerTask;
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		public ConnectionRequestor(Uri baseUri, HttpHeaders defaultHeaders, int cooldownInMilliseconds)
		{
			BaseUri = baseUri;
			_defaultHeaders = defaultHeaders;
			_cooldownInMilliseconds = cooldownInMilliseconds;

			_workerTask = Task.Run(Process);
		}

		public ConnectionRequestor(Connection connection)
			: this(connection.BaseUri, connection.DefaultHeaders, 1000 / (connection.RequestsCountPerMinute ?? 1000))
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
