namespace Jimx.WebAggregator.Parser.Html.Models;

public class RowField
{
	public int Index { get; init; }
	public string? Name { get; init; }
	public string CellText { get; init; }

	public bool IsToSerialise => Name != null;

	public RowField(int index, string? name, string cellText)
	{
		Index = index;
		Name = name;
		CellText = cellText;
	}
}
