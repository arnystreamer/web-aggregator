using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Def
{
	public interface IAuxDataProvider
	{
		string? GetAuxDataValueFromDocument(HtmlNode documentNode);
	}
}
