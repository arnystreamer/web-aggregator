using Jimx.WebAggregator.Domain.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jimx.WebAggregator.Domain.CityCosts
{
	public record City(string Name, string Region, string Country, IEnumerable<CityDataItem> DataItems) : IMongoEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? ObjectId { get; set; }
	};
}
