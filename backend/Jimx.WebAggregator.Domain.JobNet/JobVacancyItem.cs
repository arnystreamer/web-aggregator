using Jimx.WebAggregator.Domain.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jimx.WebAggregator.Domain.JobNet;

public record JobVacancyItem(long ExternalId, string Link, string Title, string CompanyName, string Location, int? LocationFormat, JobVacancyAuxInfo Auxiliary,
    JobVacancyDetails? Details) : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ObjectId { get; set; }
}