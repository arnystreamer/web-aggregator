using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Header.Def;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters.Header.Imp;

public class HeaderRowToFieldsConverter : IHeaderRowToFieldsConverter
{
	private readonly IRowToElementsConverter _rowConverter;
	private readonly IHeaderElementToFieldConverter _elementConverter;

	public HeaderRowToFieldsConverter(IRowToElementsConverter rowConverter, IHeaderElementToFieldConverter elementConverter)
	{
		_rowConverter = rowConverter;
		_elementConverter = elementConverter;
	}

	public IRowFieldsIndexer GetFieldsToSerialize(HtmlNode rowNode)
	{
		var rowFields = GetFieldsToSerializeInternal(rowNode).ToArray();
		return new RowFieldsIndexer(rowFields, false);
	}

	private IEnumerable<RowField> GetFieldsToSerializeInternal(HtmlNode rowNode)
	{
		var headerCells = _rowConverter.GetCellElements(rowNode);

		var index = 0;
		foreach (var headerCell in headerCells)
		{
			var (cellText, fieldName) = _elementConverter.GetTextAndFieldName(headerCell, index);

			yield return new RowField(index, fieldName, cellText);

			index++;
		}
	}
}

public class StaticFieldsConverter : IHeaderRowToFieldsConverter
{
	private readonly string[] _fields;

	public StaticFieldsConverter(string[] fields)
	{
		_fields = fields;
	}

	public IRowFieldsIndexer GetFieldsToSerialize(HtmlNode rowNode)
	{
		var rowFields = GetFieldsToSerializeInternal().ToArray();
		return new RowFieldsIndexer(rowFields, true);
	}

	private IEnumerable<RowField> GetFieldsToSerializeInternal()
	{
		var index = 0;
		foreach (var header in _fields)
		{
			yield return new RowField(index, header, header);

			index++;
		}
	}
}
