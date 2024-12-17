using Jimx.WebAggregator.Domain.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jimx.WebAggregator.Domain.CityCosts
{
	public record RegionTax(string Region, string Country, decimal Fixed, TaxLevel[] Levels) : IMongoEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? ObjectId { get; set; }
	}
}
