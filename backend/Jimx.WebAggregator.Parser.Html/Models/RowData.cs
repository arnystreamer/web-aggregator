namespace Jimx.WebAggregator.Parser.Html.Models;

public class RowData
{
	public bool IsSubsectionStart { get; }
	public string?[]? Values { get; }
	public string? SubsectionName { get; }

	public RowData(string?[] values)
	{
		IsSubsectionStart = false;
		Values = values;
	}

	public RowData(string subsectionName)
	{
		IsSubsectionStart = true;
		SubsectionName = subsectionName;
	}
}