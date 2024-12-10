
using Newtonsoft.Json;

namespace Jimx.Common
{
	public class CustomIntConverter : JsonConverter<int>
	{
		public override int ReadJson(JsonReader reader, Type objectType, int existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.ValueType == typeof(double))
			{
				var doubleValue = (double)reader.Value!;

				if (doubleValue % 1 != 0)
				{
					throw new Exception($"Invalid double value: {doubleValue}");
				}
			}

			return Convert.ToInt32(reader.Value);
		}

		public override void WriteJson(JsonWriter writer, int value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
	}
}
