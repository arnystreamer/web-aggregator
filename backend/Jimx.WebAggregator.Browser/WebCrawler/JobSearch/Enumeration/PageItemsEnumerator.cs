using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.HtmlTransformers;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;
using Jimx.WebAggregator.Browser.WebCrawler.Page;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using ListItemTransformer = Jimx.WebAggregator.Browser.WebCrawler.JobSearch.HtmlTransformers.ListItemTransformer;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;

public class PageItemsEnumerator : IAsyncEnumerator<JobItem>
{
    private readonly ILogger _logger;
    private readonly ILocator _ulLocator;
    private readonly HtmlBlock _jobDetailsBlock;
    private readonly PageItemsEnumeratorOptions _options;

    private readonly ListItemTransformer _listItemTransformer;
    private readonly ListItemDetailsTransformer _listItemDetailsTransformer;

    private int? _currentItemIndex;

    public PageItemsEnumerator(ILogger logger, ILocator ulLocator, HtmlBlock jobDetailsBlock, 
        ListItemTransformer listItemTransformer, ListItemDetailsTransformer listItemDetailsTransformer,
        PageItemsEnumeratorOptions options)
    {
        _logger = logger;
        _ulLocator = ulLocator;
        _jobDetailsBlock = jobDetailsBlock;
        _listItemTransformer = listItemTransformer;
        _listItemDetailsTransformer = listItemDetailsTransformer;
        _options = options;
    }
    
    public async ValueTask<bool> MoveNextAsync()
    {
        var assertingItemIndexValue = _currentItemIndex != null ? _currentItemIndex.Value + 1 : 0;
        _logger.LogDebug("Listing: list number {ListNumber} requested", assertingItemIndexValue);

        var anyListItemLocator = _ulLocator.Locator("li.scaffold-layout__list-item");
        var listItemCount = await anyListItemLocator.CountAsync();

        if (assertingItemIndexValue >= listItemCount)
        {
            _logger.LogInformation("Listing: assertingItemIndexValue ({AssertingItemIndexValue}) is less or equal to listItemCount ({ListItemCount}), probably you have reached end of list",
                assertingItemIndexValue, listItemCount);
            return false;
        }
        
        var currentLiLocator = anyListItemLocator.Nth(assertingItemIndexValue);
        
        var itemId = await currentLiLocator.GetAttributeAsync("data-occludable-job-id");
        if (itemId == null || !long.TryParse(itemId, out var jobId))
        {
            _logger.LogInformation("Listing: list item has no id or there is no next list item, probably you have reached end of list");
            return false;
        }
        
        var htmlBlock = new HtmlBlock(currentLiLocator);
        var listItem = await htmlBlock.TransformAsync(_listItemTransformer);
        
        if (listItem == null)
        {
            _logger.LogInformation("Listing: list item could not be parsed properly, probably you have reached end of list");
            return false;
        }

        if (assertingItemIndexValue % 5 == 0)
        {
            var bottomLiLocator = anyListItemLocator.Nth(Math.Min(assertingItemIndexValue + 6, listItemCount - 1));
            Task.WaitAll(
                bottomLiLocator.ScrollIntoViewIfNeededAsync(),
                Task.Delay(_options.MinimalWaitAfterNetworkOperation)
            );
        }

        _currentItemIndex = assertingItemIndexValue;
        
        JobListDetails? details = null;
        if (_options.ExpandSpecification.NeedsExpand(listItem))
        {
            _logger.LogInformation("Listing: specification indicates to expand this job");
            
            Task.WaitAll(
                currentLiLocator.ClickAsync(), 
                Task.Delay(_options.MinimalWaitAfterNetworkOperation));
            
            details = await _jobDetailsBlock.TransformAsync(_listItemDetailsTransformer);
        }
        else
        {
            _logger.LogInformation("Listing: specification indicates not to expand this job");
        }
        
        Current = new JobItem(jobId, listItem, details);
        _logger.LogDebug("Listing: current list item ready to be processed");
        return true;
    }
    
    public async ValueTask DisposeAsync()
    {
        await ValueTask.CompletedTask;
    }

    public JobItem Current { get; private set; }
}