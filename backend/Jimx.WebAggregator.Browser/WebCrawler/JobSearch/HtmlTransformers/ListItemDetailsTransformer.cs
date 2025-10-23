using Jimx.WebAggregator.Browser.WebCrawler.HtmlTransformers;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;
using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.HtmlTransformers;

public class ListItemDetailsTransformer : HtmlBlockTransformer<JobListDetails>
{
    public ListItemDetailsTransformer(ILogger logger) : base(logger)
    {
    }

    public override async Task<JobListDetails?> TransformAsync(string? html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return null;
        }

        try
        {
            return new JobListDetails(html);
        }
        catch (Exception exception)
        {
            Logger.LogError("ListItemTransformer: error when processing list item: {ExceptionMessage}. See next message for errornous HTML", exception.Message);
            Logger.LogInformation("{HTML}", html);
            
            throw;
        }
    }
}