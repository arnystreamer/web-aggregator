namespace Jimx.WebAggregator.Parser.Html.Models;

public class RowField
{
	public int Index { get; }
	public string? Name { get; }
	public string CellText { get; }

	public bool IsToSerialise => Name != null;

	public RowField(int index, string? name, string cellText)
	{
		Index = index;
		Name = name;
		CellText = cellText;
	}
}
