namespace Jimx.WebAggregator.Parser.Html.Models
{
	public class RowData
	{
		public bool IsSubsectionStart { get; set; }
		public string?[]? Values { get; set; }
		public string? SubsectionName { get; set; }

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
}
