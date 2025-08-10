using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Imp;

public class DataRowToValuesConverter : IDataRowToValuesConverter
{
	private readonly IRowToElementsConverter _rowConverter;
	private readonly IRowSubsectionChecker? _subsectionChecker;
	private readonly IDataElementToValueConvertersProvider _elementConverterProvider;

	public bool HasSubsectionChecker => _subsectionChecker != null;

	public DataRowToValuesConverter(IRowToElementsConverter rowConverter, IRowSubsectionChecker? subsectionChecker, IDataElementToValueConvertersProvider elementConverterProvider)
	{
		_rowConverter = rowConverter;
		_subsectionChecker = subsectionChecker;
		_elementConverterProvider = elementConverterProvider;
	}

	public RowData GetValues(HtmlNode dataRowNode, IRowFieldsIndexer rowFieldsIndexer)
	{
		if (_subsectionChecker != null && _subsectionChecker.IsSubsectionRow(dataRowNode))
		{
			return _subsectionChecker.GetSubsectionData(dataRowNode);
		}

		var dataCells = _rowConverter.GetCellElements(dataRowNode).ToArray();

		var result = new string?[dataCells.Length];
		for (var index = 0; index < dataCells.Length; index++)
		{
			var rowField = rowFieldsIndexer[index];
			if (rowField == null)
			{
				result[index] = null;
			}
			else
			{
				result[index] = _elementConverterProvider.GetConverter(rowField).GetDataValue(dataCells[index]);
			}
		}

		return new RowData(result);
	}
}
