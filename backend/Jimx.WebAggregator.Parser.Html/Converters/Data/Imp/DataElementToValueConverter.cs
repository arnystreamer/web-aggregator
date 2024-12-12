using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Imp
{
	public class DataElementToValueConverter : IDataElementToValueConverter
	{
		private readonly string[] _defaultValues;
		private readonly Func<HtmlNode, string?> _textSelector;

		public DataElementToValueConverter(string[] defaultValues, Func<HtmlNode, string?>? textSelector = null)
		{
			_defaultValues = defaultValues ?? [];
			_textSelector = textSelector ?? ((n) => n.LastChild.InnerText.Trim());
		}

		public string? GetDataValue(HtmlNode dataCellNode)
		{
			var innerText = _textSelector(dataCellNode);
			return _defaultValues.Contains(innerText) ? null : innerText;
		}
	}
}
