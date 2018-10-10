namespace WebAggregator.External.API.Models
{
	public class Version
	{
		public string Product { get; set; }
		public string VersionString { get; set; }

		public Version(string product, string version)
		{
			Product = product;
			VersionString = version;
		}
	}
}
