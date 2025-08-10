namespace Jimx.WebAggregator.API.Options;

public class GeneralOptions
{
	public const string OptionName = "General";

	public AuthGeneralOptions Auth { get; init; } = new();
	public string BaseUrl { get; init; } = string.Empty;
	public string FrontendUrl { get; init; } = string.Empty;
}