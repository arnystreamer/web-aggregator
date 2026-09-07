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

var parser = new Parser();

/*await parser.DoJobAsync(new LifeLevelAnalyzeJob(
    logger,
    config.GetPersistencyOptions<LifeLevelPersistencyOptions>(LifeLevelParsingJob.ConfigurationName)));*/

/*await parser.DoJobAsync(new LifeLevelReconcileJob(
    logger,
    config.GetPersistencyOptions<LifeLevelPersistencyOptions>(LifeLevelParsingJob.ConfigurationName)));*/

await parser.DoJobAsync(new LifeLevelParsingJob(
    logger,
    config.GetParsingWebsiteOptionsWithAdditionalData<LifeLevelParsingJob, LifeLevelAdditionalData>(),
    config.GetPersistencyOptions<LifeLevelPersistencyOptions>(LifeLevelParsingJob.ConfigurationName)));

/*await parser.DoJobAsync(new JobNetParsingJob(
    logger, 
    config.GetParsingWebsiteOptionsWithAdditionalData<JobNetParsingJob, JobNetAdditionalData>(),
    config.GetPersistencyOptions<JobNetPersistencyOptions>(JobNetParsingJob.ConfigurationName)));*/