using Jimx.WebAggregator.API.Options;
using Microsoft.Extensions.Options;
using static System.Linq.Enumerable;

namespace Jimx.WebAggregator.API.Services
{
	public class KeysProvider
	{
		public KeysProvider(IOptions<GeneralOptions> options)
		{
			Random rnd;
			string GetRandom()
			{
				rnd = new Random();
				return Range(1, 4).Select(x => rnd.Next().ToString("X8")).Aggregate((a, b) => a + b);
			}

			CredentialsSigningKey = options.Value.Auth?.CredentialsSigningKey ?? GetRandom();
		}

		public string CredentialsSigningKey { get; init; }
		public string IssuerSigningKey { get; init; }
	}
}
