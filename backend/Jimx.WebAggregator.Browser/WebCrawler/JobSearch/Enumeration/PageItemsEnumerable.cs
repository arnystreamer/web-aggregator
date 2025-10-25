using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.HtmlTransformers;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;
using Jimx.WebAggregator.Browser.WebCrawler.Page;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;

public class PageItemsEnumerable(ILogger logger, ILocator ulLocator, HtmlBlock jobDetailsBlock,
    ListItemTransformer listItemTransformer, ListItemDetailsTransformer listItemDetailsTransformer,
    PageItemsEnumeratorOptions enumeratorOptions) 
    : IAsyncEnumerable<JobItem>
{
    public IAsyncEnumerator<JobItem> GetAsyncEnumerator(CancellationToken cancellationToken = new ())
    {
        return new PageItemsEnumerator(logger, ulLocator, jobDetailsBlock, listItemTransformer, listItemDetailsTransformer, enumeratorOptions);
    }
}