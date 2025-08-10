using Jimx.WebAggregator.API.Options;
using Microsoft.Extensions.Options;

namespace Jimx.WebAggregator.API.Services;

public class KeysProvider
{
	public KeysProvider(IOptions<GeneralOptions> options)
	{
		CredentialsSigningKey = options.Value.Auth.CredentialsSigningKey;
	}

	public string CredentialsSigningKey { get; }
	public string IssuerSigningKey { get; init; } = string.Empty;
}