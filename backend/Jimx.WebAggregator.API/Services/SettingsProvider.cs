using Jimx.WebAggregator.API.Options;
using Microsoft.Extensions.Options;

namespace Jimx.WebAggregator.API.Services;

public class SettingsProvider
{
	public string AuthIssuer { get; }
	public string BaseUrl { get; }
	public string FrontendUrl { get; }

	public SettingsProvider(IOptions<GeneralOptions> options)
	{
		AuthIssuer = (!string.IsNullOrWhiteSpace(options.Value.Auth.Issuer) ? options.Value.Auth.Issuer :
			             Environment.GetEnvironmentVariable("GENERAL_AUTH_ISSUER"))
		             ?? "WebAggregator.API";

		BaseUrl = (!string.IsNullOrWhiteSpace(options.Value.BaseUrl) ? options.Value.BaseUrl :
			          Environment.GetEnvironmentVariable("GENERAL_BASEURL"))
		          ?? "https://localhost:55575";

		FrontendUrl = (!string.IsNullOrWhiteSpace(options.Value.FrontendUrl) ? options.Value.FrontendUrl :
			              Environment.GetEnvironmentVariable("GENERAL_FRONTENDURL"))
		              ?? "https://localhost:4200";
	}
}