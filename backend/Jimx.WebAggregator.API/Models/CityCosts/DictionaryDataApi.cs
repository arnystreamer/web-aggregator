using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Jimx.WebAggregator.API.Models.CityCosts
{
	public class DictionaryDataApi
	{
		public string Id { get; set; }

		public string? Key { get; set; }
		public string Value { get; set; }
	}
}
