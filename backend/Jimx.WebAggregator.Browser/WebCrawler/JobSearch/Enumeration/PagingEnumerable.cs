using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;

public class PagingEnumerable(ILogger logger, ILocator layoutLocator, PagingEnumeratorOptions enumeratorOptions) 
    : IAsyncEnumerable<PageItemsEnumerable>
{
    public IAsyncEnumerator<PageItemsEnumerable> GetAsyncEnumerator(CancellationToken cancellationToken = new ())
    {
        return new PagingEnumerator(logger, layoutLocator, enumeratorOptions);
    }
}