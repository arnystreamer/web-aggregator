namespace Jimx.WebAggregator.Domain.CityCosts;

public record IncomeTaxLevel(decimal From, decimal? To, decimal? Rate, decimal? RateFrom, decimal? RateTo);