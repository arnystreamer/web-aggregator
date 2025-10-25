using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.HtmlTransformers;
using Jimx.WebAggregator.Browser.WebCrawler.Page;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Enumeration;

public class PagingEnumerator : IAsyncEnumerator<PageItemsEnumerable>
{
    private readonly ILogger _logger;
    private readonly PagingEnumeratorOptions _options;
    private readonly ILocator _listContainerScrollable;

    private readonly ILocator _listLocator;

    private readonly HtmlBlock _jobDetailsBlock;

    private readonly ListItemTransformer _listItemTransformer;
    private readonly ListItemDetailsTransformer _listItemDetailsTransformer;
    
    private int? _currentPageNumber;

    public PagingEnumerator(ILogger logger, ILocator layoutLocator, PagingEnumeratorOptions options)
    {
        _logger = logger;
        _options = options;
        _listContainerScrollable = layoutLocator.Locator("div.scaffold-layout__list > div");
        _listLocator = layoutLocator.Locator("div.scaffold-layout__list > div > ul");
        _jobDetailsBlock = new HtmlBlock(layoutLocator.Locator("div.scaffold-layout__detail"));

        _listItemTransformer = new ListItemTransformer(_logger);
        _listItemDetailsTransformer = new ListItemDetailsTransformer(_logger);
    }

    public async ValueTask DisposeAsync()
    {
        await ValueTask.CompletedTask;
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        if (_currentPageNumber == null)
        {
            _logger.LogDebug("Paging: first page requested and ready");
            _currentPageNumber = 1;
            
        } 
        else
        {
            var assertingPageNumberValue = _currentPageNumber + 1;
            _logger.LogDebug("Paging: page {PageNumber} requested", assertingPageNumberValue);
            
            var paginationDiv = _listContainerScrollable.Locator("div#jobs-search-results-footer > div.jobs-search-pagination");
            var nextButton = paginationDiv.Locator("button.jobs-search-pagination__button--next");
            var nextButtonCount = await nextButton.CountAsync();
            
            switch (nextButtonCount)
            {
                case > 1:
                    _logger.LogError("Paging: more than one Next button found, actual number of buttons: {ButtonCount}", nextButtonCount);
                    throw new Exception("More than one next buttons found");
                case 0:
                    _logger.LogInformation("Paging: next button not found, it was last page");
                    return false;
            }
            
            _logger.LogDebug("Paging: next button located, ready to click");

            var clickTask = nextButton.ClickAsync();
            var delayTask = Task.Delay(_options.MinimalWaitAfterNetworkOperation);
            Task.WaitAll(clickTask, delayTask);

            await paginationDiv.ScrollIntoViewIfNeededAsync();
            
            _logger.LogDebug("Paging: next button was clicked, waited for page to change, scrolled to pagination div for further checks");
            
            var activePageButton = paginationDiv.Locator(
                "ul.jobs-search-pagination__pages > li.jobs-search-pagination__indicator > button.jobs-search-pagination__indicator-button--active",
                new LocatorLocatorOptions { HasText = assertingPageNumberValue.ToString() });

            var activePageButtonCount = await activePageButton.CountAsync();

            if (activePageButtonCount != 1)
            {
                throw new Exception($"More than one or zero active page buttons found, actual count {activePageButtonCount}");
            }
            
            _logger.LogDebug("Paging: checks completed successfully");
            _currentPageNumber++;
        }
        
        Current = new PageItemsEnumerable(_logger, _listLocator, _jobDetailsBlock, 
            _listItemTransformer, _listItemDetailsTransformer, _options.ItemEnumeratorOptions);
        _logger.LogDebug("Paging: current page ready to be processed");
        return true;
    }

    public PageItemsEnumerable Current { get; private set; }
}