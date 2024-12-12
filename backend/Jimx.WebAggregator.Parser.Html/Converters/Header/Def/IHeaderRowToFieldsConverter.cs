using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters.Header.Def;

public interface IHeaderRowToFieldsConverter
{
	IRowFieldsIndexer GetFieldsToSerialize(HtmlNode rowNode);
}
