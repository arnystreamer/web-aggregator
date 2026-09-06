using Jimx.WebAggregator.Domain.CityCosts;
using Jimx.WebAggregator.DownloadDataApp.Options;
using Jimx.WebAggregator.Parser;
using Jimx.WebAggregator.Persistent.MongoDB;
using Jimx.WebAggregator.Persistent.MongoDB.Operations;
using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.DownloadDataApp;

public class LifeLevelAnalyzeJob : IParsingJob 
{
    private readonly ILogger _logger;
    private readonly PersistencyOptionsBundle<LifeLevelPersistencyOptions> _persistencyOptions;

    public LifeLevelAnalyzeJob(
        ILogger logger,
        PersistencyOptionsBundle<LifeLevelPersistencyOptions> persistencyOptions)
    {
        _logger = logger;
        _persistencyOptions = persistencyOptions;
    }

    public static string ConfigurationName => "LifeLevelAnalyzeJob";
    public async Task DoAsync()
    {
        var server = _persistencyOptions.Main.Server;
        var databaseName = _persistencyOptions.Subsection.DatabaseName;
        var costDictionaryCollection = _persistencyOptions.Subsection.CostDictionaryCollection;
        var costCollection = _persistencyOptions.Subsection.CostCollection;

        var dictionaryConnection = new MongoConnection(new MongoOptions(_logger, server, databaseName, costDictionaryCollection));
        var citiesConnection = new MongoConnection(new MongoOptions(_logger, server, databaseName, costCollection));

        CityDictionaryItem[]? dictionaryItems = null;
        IEnumerable<CityCostsItem>? cityItems = null;

        try
        {
            var result = await dictionaryConnection.DoWorkAsync(
                dictionaryConnection.GetCollectionConnection<CityDictionaryItem>(),
                new GetMongoUnitOfWork<CityDictionaryItem>());

            if (result.IsFailure)
            {
                _logger.LogError("Getting dictionary items ended with failure");
                return;
            }

            dictionaryItems = result.AllItems.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Getting dictionary items ended with failure");
        }

        try
        {
            var result = await citiesConnection.DoWorkAsync(
                citiesConnection.GetCollectionConnection<CityCostsItem>(),
                new GetMongoUnitOfWork<CityCostsItem>());

            if (result.IsFailure)
            {
                _logger.LogError("Getting cost items ended with failure");
                return;
            }

            cityItems = result.AllItems;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Getting cost items ended with failure");
        }

        if (cityItems == null || dictionaryItems == null)
        {
            _logger.LogError("CityItems or DictionaryItems is empty");
            return;
        }

        foreach (var city in cityItems)
        {
            var missingDataItemValues = city.DataItems.Where(di => !di.Value.HasValue).ToArray();

            if (missingDataItemValues.Any())
            {
                var notMissingDataItemValues = city.DataItems.Where(di => di.Value.HasValue).ToArray();
                
                _logger.LogInformation("City {City} at {M}/{Y} has missing {DataItemKeys}, but has {DataItemKeys2}", city.Name, city.Month, city.Year,
                    string.Join(", ", missingDataItemValues.Select(di => di.DictionaryId?.ToString() ?? "null")),
                    string.Join(", ", notMissingDataItemValues.Select(di => di.DictionaryId?.ToString() ?? "null")));
            }
        }
        
        Thread.Sleep(1000);
    }
}