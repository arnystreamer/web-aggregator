using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Def;

public interface IDataElementToValueConverter
{
	string? GetDataValue(HtmlNode dataCellNode);
}
