using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html.Converters;

public interface IRowToElementsConverter
{
	IEnumerable<HtmlNode> GetCellElements(HtmlNode rowNode);
}
