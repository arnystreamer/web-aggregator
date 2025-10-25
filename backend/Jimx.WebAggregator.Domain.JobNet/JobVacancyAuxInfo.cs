namespace Jimx.WebAggregator.Domain.JobNet;

public record JobVacancyAuxInfo(bool IsVerified, DateTime ApproximateCreateTime, bool IsEasyApply, bool IsEarlyApplicant, bool IsPromoted, string? Metadata);