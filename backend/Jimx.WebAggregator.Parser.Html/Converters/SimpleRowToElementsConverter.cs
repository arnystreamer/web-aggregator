using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html.Converters;

public class SimpleRowToElementsConverter : IRowToElementsConverter
{
	private readonly string _elementName;

	public SimpleRowToElementsConverter(string elementName = "td")
	{
		_elementName = elementName;
	}

	public IEnumerable<HtmlNode> GetCellElements(HtmlNode rowNode)
	{
		return rowNode.QuerySelectorAll(_elementName);
	}
}
