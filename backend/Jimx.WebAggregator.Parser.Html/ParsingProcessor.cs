using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html;

public class ParsingProcessor
{
	public DataSet Process(string html, SourceOptions sourceOptions)
	{
		var htmlDoc = new HtmlDocument();
		htmlDoc.LoadHtml(html);

		var document = htmlDoc.DocumentNode;

		var tables = document.QuerySelectorAll(sourceOptions.TableFilter.Selector) 
			?? throw new ArgumentException("No table found for the given selector.");
		
		var table = tables.FirstOrDefault(t => t != null && sourceOptions.TableFilter.Filter(t)) 
		            ?? throw new ArgumentException("No table found for the given selector.");

		var rows = table.QuerySelectorAll("tr").ToArray()
			?? throw new Exception("No rows in table");

		var firstRow = rows.First();
		var fields = sourceOptions.RowToFieldsConverter.GetFieldsToSerialize(firstRow);

		DataSet dataSet = fields.CreateDataSet();

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

				throw new InvalidOperationException($"Number of values and fields mismatch: row #{index + 1}");
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

		PopulateAuxiliaryData(ref dataSet, sourceOptions.AuxDataSelectorsProvider, document);

		return dataSet;
	}

	private void PopulateAuxiliaryData(ref DataSet dataSet, IAuxDataSelectorsProvider? auxDataProvider, HtmlNode documentNode)
	{
		if (auxDataProvider == null)
		{
			return;
		}

		IList<AuxiliaryDataItem> auxiliaryData = new List<AuxiliaryDataItem>();
		foreach (var provider in auxDataProvider.Providers)
		{
			try
			{
				var auxDataValue = provider.Value.GetAuxDataValueFromDocument(documentNode);
				auxiliaryData.Add(new AuxiliaryDataItem(provider.Key, auxDataValue));
			}
			catch
			{
				// ignored
			}
		}

		dataSet.PopulateAuxiliaryData(auxiliaryData.ToArray());
		
	}
}
