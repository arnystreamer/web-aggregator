using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html
{
	public interface ITableFilter
	{
		bool Filter(HtmlNode tableNode);
	}
}
