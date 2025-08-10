using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Imp;

public abstract class DataElementToValueConvertersProvider : IDataElementToValueConvertersProvider
{
	protected IDictionary<string, IDataElementToValueConverter> FieldConverters = new Dictionary<string, IDataElementToValueConverter>();

	public virtual IDataElementToValueConverter GetConverter(RowField rowField)
	{
		var fieldName = rowField.Name;

		if (fieldName == null)
		{
			throw new InvalidOperationException("Cannot provide converter for null-field");
		}

		IDataElementToValueConverter? converter;
		if (!FieldConverters.TryGetValue(fieldName, out converter))
		{
			throw new Exception("Converter not found");
		}

		return converter;
	}
}