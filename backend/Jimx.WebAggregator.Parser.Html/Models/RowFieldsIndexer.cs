namespace Jimx.WebAggregator.Parser.Html.Models;

public class RowFieldsIndexer : IRowFieldsIndexer
{
	private readonly RowField?[] _rowFields;
	private readonly int _length;

	public RowFieldsIndexer(IEnumerable<RowField> rowFields, bool isFirstRowData) 
	{
		_length = rowFields.Max(x => x.Index) + 1;
		_rowFields = new RowField?[_length];
		IsFirstRowData = isFirstRowData;

		foreach (RowField rowField in rowFields.Where(r => r.IsToSerialise))
		{
			_rowFields[rowField.Index] = rowField;
		}
	}

	public RowField? this[int index] => _rowFields[index];

	public int Length => _length;

	public bool IsFirstRowData { get; private set; }

	public DataSet CreateDataSet()
	{
		return new DataSet(_rowFields);
	}
}