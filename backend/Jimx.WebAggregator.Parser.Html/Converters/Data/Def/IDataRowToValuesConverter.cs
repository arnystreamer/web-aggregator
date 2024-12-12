using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Def;

public interface IDataRowToValuesConverter
{
	bool HasSubsectionChecker { get; }
	RowData GetValues(HtmlNode dataRowNode, IRowFieldsIndexer rowFieldsIndexer);
}
