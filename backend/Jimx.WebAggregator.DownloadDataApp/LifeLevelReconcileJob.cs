using Jimx.WebAggregator.Domain.CityCosts;
using Jimx.WebAggregator.DownloadDataApp.Options;
using Jimx.WebAggregator.Parser;
using Jimx.WebAggregator.Persistent.MongoDB;
using Jimx.WebAggregator.Persistent.MongoDB.Operations;
using Microsoft.Extensions.Logging;

namespace Jimx.WebAggregator.DownloadDataApp;

public class LifeLevelReconcileJob : IParsingJob 
{
    private readonly ILogger _logger;
    private readonly PersistencyOptionsBundle<LifeLevelPersistencyOptions> _persistencyOptions;

    public LifeLevelReconcileJob(
        ILogger logger,
        PersistencyOptionsBundle<LifeLevelPersistencyOptions> persistencyOptions)
    {
        _logger = logger;
        _persistencyOptions = persistencyOptions;
    }

    public static string ConfigurationName => "LifeLevelReconcile";
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
        
        var updatedCities = new List<CityCostsItem>();

        foreach (var city in cityItems)
        {
            var missingDictionaryIdDataItems = city.DataItems.Where(di => !di.DictionaryId.HasValue).ToList();

            if (missingDictionaryIdDataItems.Any())
            {
                foreach (var dataItem in missingDictionaryIdDataItems)
                {
                    var matchingDictionaryItems = dictionaryItems.Where(di => di.Value == dataItem.Key).ToList();

                    _logger.LogInformation("Found empty item {DataItemKey} in {City} at {M}/{Y}, matched {MatchedKeys}",
                        dataItem.Key, city.Name, city.Month, city.Year,
                        string.Join(",", matchingDictionaryItems.Select(di => di.Key?.ToString() ?? "null")));

                    var singleMatchingDictionaryItem = matchingDictionaryItems.Single();

                    if (!singleMatchingDictionaryItem.Key.HasValue)
                    {
                        throw new Exception();
                    }

                    dataItem.DictionaryId = singleMatchingDictionaryItem.Key.Value;
                }
                
                updatedCities.Add(city);
            }
            else
            {
                _logger.LogInformation("Everything is ok for {City} at {M}/{Y}", city.Name, city.Month, city.Year);
            }
        }

        if (updatedCities.Any())
        {
            if (updatedCities.Any(c => c.DataItems.Any(di => !di.DictionaryId.HasValue)))
            {
                throw new Exception("There are cities with empty data items dictionary keys");
            }
            
            _logger.LogInformation("Updating {Count} cities...", updatedCities.Count);

            await citiesConnection.DoWorkAsync(
                citiesConnection.GetCollectionConnection<CityCostsItem>(),
                new UpsertMongoUnitOfWork<CityCostsItem, CityKey>(updatedCities,
                    new UpsertOptions<CityCostsItem, CityKey>(
                        c => new CityKey(c.Name, c.Region, c.Country, c.Month, c.Year),
                        id => c => c.Name == id.Name && c.Region == id.Region && c.Country == id.Country && c.Month == id.Month && c.Year == id.Year, 
                        null,
                        true
                        ))
            );
        }
        else
        {
            _logger.LogInformation("No cities to be updated");
        }
        
        Thread.Sleep(1000);
    }
}