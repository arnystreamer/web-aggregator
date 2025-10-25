using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Jimx.WebAggregator.Browser.WebCrawler.HtmlTransformers;

public abstract class HtmlBlockTransformer<T>
{
    protected readonly ILogger Logger;

    protected HtmlBlockTransformer(ILogger logger)
    {
        Logger = logger;
    }

    public async Task<T?> TransformAsync(ILocator locator)
    {
        var innerHtml = await locator.InnerHTMLAsync();
        return await TransformAsync(innerHtml);
    }
    public abstract Task<T?> TransformAsync(string? html);
}