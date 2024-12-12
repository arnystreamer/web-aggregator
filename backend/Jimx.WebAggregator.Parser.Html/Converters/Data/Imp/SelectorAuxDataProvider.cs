using HtmlAgilityPack;
using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Imp
{
	public class SelectorAuxDataProvider : IAuxDataProvider
	{
		private readonly Func<HtmlNode, string?> _selectorFunc;

		public SelectorAuxDataProvider(Func<HtmlNode, string?> selectorFunc)
		{
			_selectorFunc = selectorFunc ?? throw new ArgumentNullException(nameof(selectorFunc));
		}

		public string? GetAuxDataValueFromDocument(HtmlNode documentNode)
		{
			return _selectorFunc.Invoke(documentNode);
		}
	}
}
