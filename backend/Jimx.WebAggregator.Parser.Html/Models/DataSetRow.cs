namespace Jimx.WebAggregator.Parser.Html.Models
{
	public class DataSetRow
	{
		public string? Subsection { get; init; }
		public string?[] Data { get; init; }

		public DataSetRow(string? subsection, string?[] data)
		{
			Subsection = subsection;
			Data = data;
		}
	}

	public record AuxiliaryDataItem(string Key, string? Value);
}
