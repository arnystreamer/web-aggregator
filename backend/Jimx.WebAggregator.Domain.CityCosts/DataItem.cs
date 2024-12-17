using Jimx.WebAggregator.Domain.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jimx.WebAggregator.Domain.CityCosts
{
	public record DataItem(string? Key, string Value) : IMongoEntity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? ObjectId { get; set; }
	};
}
