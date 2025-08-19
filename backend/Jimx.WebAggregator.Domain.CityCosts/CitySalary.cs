using Jimx.WebAggregator.Domain.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jimx.WebAggregator.Domain.CityCosts;

public record CitySalary(string City, CitySalaryItem[] Items) : IMongoEntity
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string? ObjectId { get; set; }
}

public record CitySalaryItem(decimal P25, decimal P75, int? Score, string? Source, string? Comment);