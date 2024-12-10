using Jimx.Common;

namespace Jimx.WebAggregator.Parser.Http
{
    public class ConnectionRequestor
    {
        public readonly Uri BaseUri;
        private readonly HttpHeaders _defaultHeaders;
        private readonly int _cooldownInMilliseconds;

        private readonly Queue<QueueItem> _queueItems = new Queue<QueueItem>();

        public ConnectionRequestor(Uri baseUri, HttpHeaders defaultHeaders, int cooldownInMilliseconds)
        {
            BaseUri = baseUri;
            _defaultHeaders = defaultHeaders;
            _cooldownInMilliseconds = cooldownInMilliseconds;
        }

        public ConnectionRequestor(Connection connection)
            : this(connection.BaseUri, connection.DefaultHeaders, 1000 / (connection.RequestsCountPerMinute ?? 1000))
        {

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

        private record QueueItem(HttpRequestMessage Message, TaskCompletionSource<HttpResponseMessage> TaskCompletionSource);
    }
}
