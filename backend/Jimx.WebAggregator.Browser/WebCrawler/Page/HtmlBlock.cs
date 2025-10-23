using Jimx.WebAggregator.Browser.WebCrawler.HtmlTransformers;
using Microsoft.Playwright;

namespace Jimx.WebAggregator.Browser.WebCrawler.Page;

public class HtmlBlock(ILocator locator)
{
    public async Task<HtmlSnapshot> GetHtmlSnapshot()
    {
        var html = await locator.InnerHTMLAsync();
        return new HtmlSnapshot(html);
    }

    public async Task<T?> TransformAsync<T>(HtmlBlockTransformer<T> htmlBlockTransformer)
    {
        return await htmlBlockTransformer.TransformAsync(locator);
    }
}