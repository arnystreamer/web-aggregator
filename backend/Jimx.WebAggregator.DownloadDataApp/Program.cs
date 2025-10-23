using Jimx.WebAggregator.Parser;
using Jimx.WebAggregator.DownloadDataApp;
using Jimx.WebAggregator.DownloadDataApp.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var factory = LoggerFactory.Create(builder => builder
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole());
var logger = factory.CreateLogger("Jimx.WebAggregator.DownloadDataApp");

var config =
    new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

var parsingWebSiteOptions = config.GetSection("ParsingWebSiteOptions").Get<ParsingWebSiteOptions>();

if (parsingWebSiteOptions == null)
{
    throw new ApplicationException("ParsingWebSiteOptions is null");
}

await new Parser().DoJobAsync(new JobNetParsingJob(logger, parsingWebSiteOptions));