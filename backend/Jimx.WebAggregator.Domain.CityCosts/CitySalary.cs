using Jimx.WebAggregator.Domain.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jimx.WebAggregator.Domain.CityCosts
{
	public record CitySalary(string City, decimal? P25, decimal? P75) : IMongoEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? ObjectId { get; set; }
	}
}
