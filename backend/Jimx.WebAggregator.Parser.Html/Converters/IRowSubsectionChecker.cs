using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters;

public interface IRowSubsectionChecker
{
	bool IsSubsectionRow(HtmlNode dataRowNode);
	RowData GetSubsectionData(HtmlNode dataRowNode);
}
