using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Jimx.WebAggregator.API.Models.CityCosts
{
	public class CityApi
	{
		public string Id { get; set; }

		public string Name { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }

		public CityDataItemApi[] DataItems { get; set; }
	}
}
