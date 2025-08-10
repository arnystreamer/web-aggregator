using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Converters.Header.Def;

namespace Jimx.WebAggregator.Parser.Html;

public class SourceOptions
{
	public ITableFilter TableFilter { get; }
	public IHeaderRowToFieldsConverter RowToFieldsConverter { get; }
	public IDataRowToValuesConverter DataRowToValuesConverter { get; }
	public IAuxDataSelectorsProvider? AuxDataSelectorsProvider { get; }

	public bool IgnoreRowsWithCellsDiscrepancies { get; init; }
	public bool ExpectNullsInSubsectionNames { get; init; }

	public SourceOptions(ITableFilter tableFilter, IHeaderRowToFieldsConverter rowToFieldsConverter, IDataRowToValuesConverter dataRowToValuesConverter, 
		IAuxDataSelectorsProvider? auxDataSelectorsProvider = null)
	{
		TableFilter = tableFilter ?? throw new ArgumentNullException(nameof(tableFilter));
		RowToFieldsConverter = rowToFieldsConverter ?? throw new ArgumentNullException(nameof(rowToFieldsConverter));
		DataRowToValuesConverter = dataRowToValuesConverter ?? throw new ArgumentNullException(nameof(dataRowToValuesConverter));

		AuxDataSelectorsProvider = auxDataSelectorsProvider;
	}
}
