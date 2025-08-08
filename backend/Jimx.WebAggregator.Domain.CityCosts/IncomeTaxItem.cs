namespace Jimx.WebAggregator.Domain.CityCosts;

public record IncomeTaxItem(string? ID, string Name, string[]? Tags, string? ApplyOn, bool? IsActive, 
    decimal? MonthlyFixedAmount, decimal? AnnualFixedAmount, decimal? Rate, decimal? Multiplier,
    decimal? FixedDeductionAmount, string? Comment,
    IncomeTaxLevel[]? Levels);