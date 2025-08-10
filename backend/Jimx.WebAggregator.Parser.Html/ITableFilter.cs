using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html;

public interface ITableFilter
{
	string Selector { get; }
	bool Filter(HtmlNode tableNode);
}