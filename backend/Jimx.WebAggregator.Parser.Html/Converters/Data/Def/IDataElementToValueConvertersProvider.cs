using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Def;

public interface IDataElementToValueConvertersProvider
{
	IDataElementToValueConverter GetConverter(RowField rowField);
}
