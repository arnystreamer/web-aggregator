using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html.Converters.Header.Def;

public interface IHeaderElementToFieldConverter
{
	(string, string?) GetTextAndFieldName(HtmlNode rowCellNode, int index);
}
