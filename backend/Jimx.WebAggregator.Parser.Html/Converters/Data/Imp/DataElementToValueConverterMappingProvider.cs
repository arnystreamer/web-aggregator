using Jimx.WebAggregator.Parser.Html.Converters.Data.Def;
using Jimx.WebAggregator.Parser.Html.Models;

namespace Jimx.WebAggregator.Parser.Html.Converters.Data.Imp;

public class DataElementToValueConverterMappingProvider : IDataElementToValueConverterProvider
{
	private readonly IDictionary<string, string[]> _fieldToDefaultValues;

	private IDictionary<string, IDataElementToValueConverter> _fieldToConverter;

	public DataElementToValueConverterMappingProvider()
		:this(new  Dictionary<string, string[]>())
	{

	}

	public DataElementToValueConverterMappingProvider(IDictionary<string, string[]> fieldToDefaultValues)
	{
		_fieldToDefaultValues = fieldToDefaultValues;
		_fieldToConverter = new Dictionary<string, IDataElementToValueConverter>();
	}

	public IDataElementToValueConverter GetConverter(RowField rowField)
	{
		var fieldName = rowField.Name;

		if (fieldName == null)
		{
			throw new InvalidOperationException("Cannot provide converter for null-field");
		}

		IDataElementToValueConverter? converter;
		if (!_fieldToConverter.TryGetValue(fieldName, out converter))
		{
			if (_fieldToDefaultValues.TryGetValue(fieldName, out var defaultValues))
			{
				converter = new DataElementToValueConverter(defaultValues);
			}
			else
			{
				converter = new DataElementToValueConverter([]);
			}

			_fieldToConverter[fieldName] = converter;
		}

		return converter;
	}
}
