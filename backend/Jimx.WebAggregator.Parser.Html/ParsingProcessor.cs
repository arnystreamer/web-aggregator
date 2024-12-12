using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Models;
using static Jimx.WebAggregator.Parser.Html.Models.DataSet;

namespace Jimx.WebAggregator.Parser.Html;

public class ParsingProcessor
{
	public DataSet Process(string html, SourceOptions sourceOptions)
	{
		var htmlDoc = new HtmlDocument();
		htmlDoc.LoadHtml(html);

		var document = htmlDoc.DocumentNode;

		var tables = document.QuerySelectorAll(sourceOptions.Selector) 
			?? throw new ArgumentException("No table found for the given selector.");
		
		HtmlNode table = tables.FirstOrDefault(t => t != null && sourceOptions.TableFilter.Filter(t)) 
			?? throw new ArgumentException("No table found for the given selector.");

		var rows = table.QuerySelectorAll("tr")
			?? throw new Exception("No rows in table");

		var firstRow = rows.First();
		var fields = sourceOptions.RowToFieldsConverter.GetFieldsToSerialize(firstRow);

		var dataSet = fields.CreateDataSet();

		var objectsRows = new List<DataSetRow>();

		var index = 0;
		string? subsectionName = null;
		foreach (var row in rows.Skip(fields.IsFirstRowData ? 0 : 1))
		{
			index++;

			var rowData = sourceOptions.DataRowToValuesConverter.GetValues(row, fields);

			if (rowData.IsSubsectionStart)
			{
				subsectionName = rowData.SubsectionName;
				continue;
			}

			if (rowData.Values!.Length != fields.Length)
			{
				if (sourceOptions.IgnoreRowsWithCellsDiscrepancies)
				{
					continue;
				}
				else
				{
					throw new InvalidOperationException($"Number of values and fields mismatch: row #{index + 1}");
				}
			}

			var fieldValues = new string?[fields.Length];

			for (int i = 0; i < fields.Length; i++)
			{
				if (fields[i]?.IsToSerialise ?? false)
				{
					fieldValues[i] = rowData.Values[i];
				}
			}

			if (sourceOptions.DataRowToValuesConverter.HasSubsectionChecker && !sourceOptions.ExpectNullsInSubsectionNames && subsectionName == null)
			{
				throw new InvalidOperationException("Subsection name is null but not expected to be null");
			}

			var objectsRow = new DataSetRow(subsectionName, fieldValues);
			objectsRows.Add(objectsRow);
		}

		dataSet.PopulateData(objectsRows.ToArray());

		IList<AuxiliaryDataItem> auxiliaryData = new List<AuxiliaryDataItem>();
		foreach (var auxDataProvider in sourceOptions.AuxDataSelectorsProvider.Providers)
		{
			try
			{
				var auxDataValue = auxDataProvider.Value.GetAuxDataValueFromDocument(document);
				auxiliaryData.Add(new AuxiliaryDataItem(auxDataProvider.Key, auxDataValue));
			}
			catch
			{
				continue;
			}
		}
		
		dataSet.PopulateAuxiliaryData(auxiliaryData.ToArray());

		return dataSet;
	}
}
