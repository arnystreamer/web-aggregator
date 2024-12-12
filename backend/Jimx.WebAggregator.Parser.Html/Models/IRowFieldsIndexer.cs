namespace Jimx.WebAggregator.Parser.Html.Models;

public interface IRowFieldsIndexer
{
	RowField? this[int index] { get; }
	int Length { get; }

	bool IsFirstRowData { get; }

	DataSet CreateDataSet();
}
