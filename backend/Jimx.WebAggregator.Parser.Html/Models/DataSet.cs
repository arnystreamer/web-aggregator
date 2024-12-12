namespace Jimx.WebAggregator.Parser.Html.Models
{
	public class DataSet
	{
		public RowField?[] RowFields { get; init; }
		public DataSetRow[] Data { get; private set; } = [];
		public AuxiliaryDataItem[] AuxiliaryData { get; private set; } = [];

		public DataSet(RowField?[] rowFields)
		{
			RowFields = rowFields;
		}

		public void PopulateData(DataSetRow[] data)
		{
			Data = data;
		}

		public void PopulateAuxiliaryData(AuxiliaryDataItem[] auxiliaryData)
		{
			AuxiliaryData = auxiliaryData;
		}

		public AuxiliaryDataItem? TryGetAuxiliaryData(string key)
		{
			return AuxiliaryData.FirstOrDefault(x => x.Key == key);
		}
	}
}
