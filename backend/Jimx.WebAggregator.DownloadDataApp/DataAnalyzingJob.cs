using Jimx.WebAggregator.Domain.CityCosts;
using Jimx.WebAggregator.DownloadDataApp.Options;
using Jimx.WebAggregator.Parser;
using Jimx.WebAggregator.Persistent.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.DownloadDataApp;

public class DataAnalyzingJob : IParsingJob
{
    private readonly ILogger _logger;
    private readonly PersistencyOptionsBundle<DataAnalyzingPersistencyOptions> _persistencyOptions;

    public DataAnalyzingJob(ILogger logger, PersistencyOptionsBundle<DataAnalyzingPersistencyOptions> persistencyOptions)
    {
        _logger = logger;
        _persistencyOptions = persistencyOptions;
    }

    public static string ConfigurationName => "DataAnalyzing";
    
    public async Task DoAsync()
    {
        var server = _persistencyOptions.Main.Server;
        var databaseName = _persistencyOptions.Subsection.DatabaseName;

        var mongoCitiesOpts = new MongoOptions(_logger, server, databaseName, _persistencyOptions.Subsection.LeftCollection);
        var citiesConnection = new MongoConnection(mongoCitiesOpts).GetCollectionConnection<CityCostsItem>();

        var citiesFindTask = citiesConnection.Collection.FindAsync(_ => true);
        
        var mongoSalariesOpts = new MongoOptions(_logger, server, databaseName, _persistencyOptions.Subsection.RightCollection);
        var salariesConnection = new MongoConnection(mongoSalariesOpts).GetCollectionConnection<CitySalary>();
        var salariesFindTask = salariesConnection.Collection.FindAsync(_ => true);
        
        var salaries = (await salariesFindTask).ToList();
        var cities = (await citiesFindTask).ToList();
        var citiesWithoutSalaries = cities.Select(c => c.Name).Except(salaries.Select(s => s.City));
        var salariesWithoutCities = salaries.Select(s => s.City).Except(cities.Select(c => c.Name));
        
        _logger.LogInformation("citiesWithoutSalaries: {cities}", string.Join(", ", citiesWithoutSalaries));
        _logger.LogInformation("salariesWithoutCities: {cities}", string.Join(", ", salariesWithoutCities));
        
        await Task.Delay(5000);
    }
}