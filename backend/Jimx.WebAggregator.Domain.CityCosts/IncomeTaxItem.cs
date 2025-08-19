namespace Jimx.WebAggregator.Domain.CityCosts;

public record IncomeTaxItem(string? ID, string Name, string[]? Tags, string? ApplyOn, int? Order, bool? IsActive, 
    decimal? MonthlyFixedAmount, decimal? AnnualFixedAmount, decimal? Rate, decimal? Multiplier,
    decimal? FixedDeductionAmount, decimal? FixedDeductionRate, string? Comment,
    IncomeTaxLevel[]? Levels);