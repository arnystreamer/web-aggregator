using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Jimx.WebAggregator.Domain.CityCosts;

public record RegionTaxDeduction(string? Region, string Country, string ID, string CurrencyCode,
    RegionTaxDeductionSource Source, IncomeTaxItem[] IncomeTaxes)
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ObjectId { get; set; }
}