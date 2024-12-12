using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Imp
{
	public class DataElementToValueConverter : IDataElementToValueConverter
	{
		private readonly Func<HtmlNode, string?> _textSelector;

		public DataElementToValueConverter(Func<HtmlNode, string?>? textSelector = null)
		{
			_textSelector = textSelector ?? ((n) => n.LastChild.InnerText.Trim());
		}

		public string? GetDataValue(HtmlNode dataCellNode)
		{
			var innerText = _textSelector(dataCellNode);
			return innerText;
		}
	}
}
