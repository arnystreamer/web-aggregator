using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Def;

public interface IDataElementToValueConverterProvider
{
	IDataElementToValueConverter GetConverter(RowField rowField);
}
