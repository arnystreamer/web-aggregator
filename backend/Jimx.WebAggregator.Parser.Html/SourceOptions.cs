using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Converters.Header.Def;

namespace Jimx.WebAggregator.Parser.Html;

public class SourceOptions
{
	public string Selector { get; init; }
	public ITableFilter TableFilter { get; init; }
	public IHeaderRowToFieldsConverter RowToFieldsConverter { get; init; }
	public IDataRowToValuesConverter DataRowToValuesConverter { get; init; }
	public IAuxDataSelectorsProvider AuxDataSelectorsProvider { get; init; }

	public bool IgnoreRowsWithCellsDiscrepancies { get; set; }
	public bool ExpectNullsInSubsectionNames { get; set; }

	public SourceOptions(string selector, ITableFilter tableFilter, IHeaderRowToFieldsConverter rowToFieldsConverter, IDataRowToValuesConverter dataRowToValuesConverter, 
		IAuxDataSelectorsProvider auxDataSelectorsProvider)
	{
		Selector = selector ?? throw new ArgumentNullException(nameof(selector));
		TableFilter = tableFilter ?? throw new ArgumentNullException(nameof(tableFilter));
		RowToFieldsConverter = rowToFieldsConverter ?? throw new ArgumentNullException(nameof(rowToFieldsConverter));
		DataRowToValuesConverter = dataRowToValuesConverter ?? throw new ArgumentNullException(nameof(dataRowToValuesConverter));
		AuxDataSelectorsProvider = auxDataSelectorsProvider ?? throw new ArgumentNullException(nameof(auxDataSelectorsProvider));
	}
}
