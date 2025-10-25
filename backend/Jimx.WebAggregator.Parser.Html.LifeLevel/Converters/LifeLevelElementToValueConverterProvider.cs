using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Imp;

namespace Jimx.WebAggregator.Parser.Html.LifeLevel.Converters;

public class LifeLevelElementToValueConverterProvider : DataElementToValueConvertersProvider
{
	public LifeLevelElementToValueConverterProvider()
	{
		FieldConverters = new Dictionary<string, IDataElementToValueConverter>
		{
			{ "Category", new DataElementToValueConverter() },
			{ "Price", new DataElementToValueConverter(GetLifeLevelPriceString ) },
			{ "Range", new DataElementToValueConverter(GetLifeLevelPriceRangeString) },
		};
	}

	private (string? Left, string? Right) GetLifeLevelPriceRange(HtmlNode node)
	{
		var leftText = node.Descendants(0).FirstOrDefault(cn => cn.HasClass("barTextLeft"))?.InnerText.Trim();
		var rightText = node.Descendants(0).FirstOrDefault(cn => cn.HasClass("barTextRight"))?.InnerText.Trim();

		return (leftText, rightText);
	}

	private string GetLifeLevelPriceRangeString(HtmlNode node)
	{
		var range = GetLifeLevelPriceRange(node);

		return $"{range.Left ?? "?"} - {range.Right ?? "?"}";
	}

	private string? GetLifeLevelPriceString(HtmlNode node)
	{
		var numberPart = node.InnerText?.Split('&', 2) ?? [];
		if (numberPart.Length == 0)
		{
			return null;
		}

		var firstPart = numberPart[0];
		return firstPart == "?" 
			? null 
			: firstPart.Trim();
	}
}