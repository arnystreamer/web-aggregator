using System.Text.RegularExpressions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Jimx.WebAggregator.Browser.WebCrawler.HtmlTransformers;
using Jimx.WebAggregator.Browser.WebCrawler.JobSearch.Models;
using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.Browser.WebCrawler.JobSearch.HtmlTransformers;

public partial class ListItemTransformer : HtmlBlockTransformer<JobListItem>
{
    public ListItemTransformer(ILogger logger) : base(logger)
    {
    }

    public override async Task<JobListItem?> TransformAsync(string? html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return null;
        }

        try
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var itemRootNode = doc.DocumentNode.QuerySelector("div > div.job-card-container > div");

            var titleLinkNode = itemRootNode.QuerySelector("div.artdeco-entity-lockup__content > div.artdeco-entity-lockup__title > a");

            var link = titleLinkNode.GetAttributeValue("href", string.Empty);
            var title = titleLinkNode.QuerySelector("span > strong").InnerText.Trim();
            var titleAria = titleLinkNode.GetAttributeValue("aria-label", string.Empty);

            var verifiedIconPicture = titleLinkNode.QuerySelectorAll("span > span > svg.text-view-model__verified-icon").FirstOrDefault();

            var subtitleNode = itemRootNode.QuerySelector("div.artdeco-entity-lockup__content > div.artdeco-entity-lockup__subtitle > span");
            var companyName = subtitleNode.InnerText.Trim();

            var locationNodes = itemRootNode.QuerySelectorAll("div.artdeco-entity-lockup__content > div.artdeco-entity-lockup__caption > ul > li").ToList();
            if (locationNodes.Count != 1)
            {
                throw new Exception("Unexpected number of locations");
            }

            var location = locationNodes.First().ChildNodes.First(n => n.Name == "span").InnerText.Trim();
            string? locationFormat = null;
            var match = InsideParenthesisRegex().Match(location);
            if (match.Success)
            {
                var captured = match.Groups[1].Value;
                var allowedLocationFormats = new[] { "On-site", "Hybrid", "Remote" };
                if (allowedLocationFormats.Contains(captured))
                {
                    locationFormat = captured;
                    location = location.Replace($"({captured})", string.Empty).Trim();
                }
            }
            
            var metadataNodes = itemRootNode.QuerySelectorAll("div.artdeco-entity-lockup__content > div.artdeco-entity-lockup__metadata > ul > li").ToList();
            string? metadata = null;
            if (metadataNodes.Count == 1)
            {
                metadata = metadataNodes.First().ChildNodes.First(n => n.Name == "span").InnerText.Trim();
            }

            var footerItemNodes = itemRootNode.QuerySelectorAll("ul.job-card-container__footer-wrapper > li");

            string? publishedAgoString = null;
            string? viewedString = null;
            bool isEasyApply = false;
            bool isPromoted = false;
            bool beAnEarlyApplicant = false;
            foreach (var footerItemNode in footerItemNodes)
            {
                if (footerItemNode.ChildNodes.All(n => n.Name == "#text"))
                {
                    viewedString = footerItemNode.InnerText.Trim();
                    continue;
                }

                var timeElement = footerItemNode.ChildNodes.FirstOrDefault(n => n.Name == "time");
                if (timeElement != null)
                {
                    publishedAgoString = timeElement.GetDirectInnerText().Trim();
                }

                var spanNodes = footerItemNode.ChildNodes.Where(n => n.Name == "span").ToList();
                if (spanNodes.Any(n => n.InnerText.Trim() == "Easy Apply"))
                {
                    isEasyApply = true;
                }
                
                if (spanNodes.Any(n => n.InnerText.Trim() == "Promoted"))
                {
                    isPromoted = true;
                }
                
                if (spanNodes.Any(n => n.GetDirectInnerText().Trim() == "Be an early applicant"))
                {
                    beAnEarlyApplicant = true;
                }
            }

            return new JobListItem(link, title, verifiedIconPicture != null, companyName, location, locationFormat, metadata,
                publishedAgoString, viewedString, isEasyApply, isPromoted, beAnEarlyApplicant, html);
        }
        catch (Exception exception)
        {
            Logger.LogError("ListItemTransformer: error when processing list item: {ExceptionMessage}. See next message for errornous HTML", exception.Message);
            Logger.LogInformation("{HTML}", html);
            
            throw;
        }
    }

    [GeneratedRegex(@"\(([^(]+)\)")]
    private static partial Regex InsideParenthesisRegex();
}