using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.LifeLevel.Converters;

public class LifeLevelRowSubsectionChecker : IRowSubsectionChecker
{
	public RowData GetSubsectionData(HtmlNode node)
	{
		return new RowData(node.Descendants().First(cn => cn.HasClass("category_title") || cn.HasClass("tr_highlighted_menu")).InnerText.Trim());
	}

	public bool IsSubsectionRow(HtmlNode node)
	{
		return node.Descendants(0).Any(cn => cn.Name == "th");
	}
}