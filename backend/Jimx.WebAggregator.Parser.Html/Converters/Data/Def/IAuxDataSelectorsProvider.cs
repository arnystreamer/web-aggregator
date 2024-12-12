namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Def
{
	public interface IAuxDataSelectorsProvider
	{
		IDictionary<string, IAuxDataProvider> Providers { get; }
	}
}
