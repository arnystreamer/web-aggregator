using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Header.Def;

namespace Jimx.WebAggregator.Parser.Html.Converters.Header.Imp;

public class HeaderElementToFieldMappingConverter : IHeaderElementToFieldConverter
{
	private readonly IDictionary<string, string?> _columnToFieldMapping;

	public HeaderElementToFieldMappingConverter(IDictionary<string, string?> columnToFieldMapping)
	{
		_columnToFieldMapping = columnToFieldMapping;
	}

	public (string, string?) GetTextAndFieldName(HtmlNode rowCellNode, int index)
	{
		var text = rowCellNode.InnerText.Trim();

		if (_columnToFieldMapping.TryGetValue(text, out string? value))
		{
			return (text, value);
		}

		return (text, text);
	}
}