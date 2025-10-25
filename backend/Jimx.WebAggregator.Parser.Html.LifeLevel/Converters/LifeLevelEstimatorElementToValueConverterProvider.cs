using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Imp;

namespace Jimx.WebAggregator.Parser.Html.LifeLevel.Converters;

public class LifeLevelEstimatorElementToValueConverterProvider : DataElementToValueConvertersProvider
{
	public LifeLevelEstimatorElementToValueConverterProvider()
	{
		FieldConverters = new Dictionary<string, IDataElementToValueConverter>()
		{
			{ "Category", new DataElementToValueConverter(GetMixedCategoryString) },
			{ "Price", new DataElementToValueConverter(GetLifeLevelPriceString) }
		};
	}

	private string? GetMixedCategoryString(HtmlNode node)
	{
		return node.FirstChild?.InnerText ?? null;
	}

	private string? GetLifeLevelPriceString(HtmlNode node)
	{
		var numberPart = node.InnerText?.Split('&', 2) ?? [];
		return numberPart.Length > 0 
			? numberPart[0].Trim() 
			: null;
	}
}