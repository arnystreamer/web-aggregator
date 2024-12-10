using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Constructor
{
    public class SimpleBuilder<TOutputItem> : IBuilder<TOutputItem>
    {
        public Requestor Requestor { get; }
        public Lazy<TOutputItem> ExecutingFactory { get; }

        public SimpleBuilder(Requestor requestor, Lazy<TOutputItem> executingFactory)
        {
            Requestor = requestor;
            ExecutingFactory = executingFactory;
        }

        public TOutputItem Execute()
        {
            return ExecutingFactory.Value;
        }
    }
}
