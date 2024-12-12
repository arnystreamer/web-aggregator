using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Imp
{
	public class SimpleAuxDataSelectorsProvider : IAuxDataSelectorsProvider
	{
		public IDictionary<string, IAuxDataProvider> Providers { get; }

		public SimpleAuxDataSelectorsProvider(IDictionary<string, IAuxDataProvider> providers)
		{
			Providers = providers;
		}
	}
}
