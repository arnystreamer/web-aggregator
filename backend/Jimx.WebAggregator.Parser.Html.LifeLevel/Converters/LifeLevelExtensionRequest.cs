using Fizzler.Systems.HtmlAgilityPack;
using Jimx.WebAggregator.Domain.CityCosts;
using Jimx.WebAggregator.Parser.Builder;
using Jimx.WebAggregator.Parser.Helpers;
using Jimx.WebAggregator.Parser.Html.Converters;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Imp;
using Jimx.WebAggregator.Parser.Html.Converters.Header.Imp;

namespace Jimx.WebAggregator.Parser.Html.LifeLevel.Converters;

public abstract class LifeLevelExtensionRequest : ExtensionRequest<CityCostsItem, CityCostsItem>
{
	protected override async Task<CityCostsItem> GetInformationFromResponse(CityCostsItem input, HttpResponseMessage message)
	{
		var html = await message.Content.ReadAsStringAsync();

		var fields = new[] { "Category", "Price", "Range" };

		IDictionary<string, IAuxDataProvider> auxProviders = new Dictionary<string, IAuxDataProvider>
		{
			{ "ActualDate", new SelectorAuxDataProvider(n =>
				n.QuerySelectorAll("div.align_like_price_table")
					.FirstOrDefault(div => div.InnerText.Contains("Last update:"))?
					.InnerText.Split("Last update:")[1].Trim() ?? null) },
			{ "NowDate", new SelectorAuxDataProvider(n => DateTime.Now.ToString("O")) }
		};

		var parsingOptions = new SourceOptions(
			tableFilter: FuncTableFilter.Create("table.data_wide_table"),
			rowToFieldsConverter: new StaticFieldsConverter(fields),
			dataRowToValuesConverter: new DataRowToValuesConverter(new SimpleRowToElementsConverter(), new LifeLevelRowSubsectionChecker(), new LifeLevelElementToValueConverterProvider()),
			new SimpleAuxDataSelectorsProvider(auxProviders))
		{
			IgnoreRowsWithCellsDiscrepancies = true,
			ExpectNullsInSubsectionNames = false
		};

		var dataSet = new ParsingProcessor().Process(html, parsingOptions);

		var actualDateString = dataSet.TryGetAuxiliaryData("ActualDate")?.Value;

		if (actualDateString != null && DateTime.TryParse(actualDateString, out var actualDate))
		{
			actualDateString = actualDate.AddMonths(1).AddDays(-1).ToString("O");
		}

		var dataSetName = GetDataSetName();
		var newDataItems = dataSet.Data
			.Select(row => new CityDataItem($"{dataSetName} - {row.Subsection} - {row.Data[0]}", row.Data[1].ParseToDecimal()))
			.ToArray();

		return new CityCostsItem(input.Name, input.Region, input.Country, input.Year, input.Month, input.DataItems.Union(newDataItems));
	}

	protected abstract string GetDataSetName();
}