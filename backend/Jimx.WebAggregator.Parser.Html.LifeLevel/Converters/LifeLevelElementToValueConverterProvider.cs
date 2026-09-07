using System.Text.RegularExpressions;
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
		var dirtyPriceText = node.InnerText?.Trim();

		if (string.IsNullOrEmpty(dirtyPriceText) || dirtyPriceText == "?")
		{
			return null;
		}
		
		var regex = new Regex(@"([\d,.]+)(&.+;|[^\d]+)*$");
		var match = regex.Match(dirtyPriceText);
		
		if (match.Success)
		{
			return match.Groups[1].Value;
		}
		
		return null;
	}
}