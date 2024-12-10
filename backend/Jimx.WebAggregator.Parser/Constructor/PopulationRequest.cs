using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Constructor
{
    public abstract class PopulationRequest<TInput, TOutput> : IPopulationRequest<TInput, TOutput>
    {
        private Requestor? _requestor = null;

        public virtual void SetRequestor(Requestor requestor)
        {
            _requestor = requestor;
        }

        public abstract Uri GetUri(TInput input);

        public virtual TOutput Request(TInput input)
        {
            if (_requestor == null)
            {
                throw new InvalidOperationException();
            }

            var uri = GetUri(input);
            return _requestor.RequestAsJson<TOutput>(uri, HttpMethod.Get, new HttpHeaders()).Result;
        }
    }
}
