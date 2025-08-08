namespace Jimx.WebAggregator.Domain.CityCosts;

public record RegionTaxDeductionSource(
    string? Main,
    string? Additional,
    string? Additional2,
    bool? IsTrustworthy,
    string? Comment);