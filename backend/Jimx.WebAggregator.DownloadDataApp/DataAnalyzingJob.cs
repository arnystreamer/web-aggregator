using Jimx.Common.Helpers.Lists;
using Jimx.WebAggregator.Domain.CityCosts;
using Jimx.WebAggregator.Parser;
using Jimx.WebAggregator.Persistent.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Jimx.WebAggregator.DownloadDataApp;

public class DataAnalyzingJob : IParsingJob
{
    public async Task DoAsync()
    {
        var factory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = factory.CreateLogger("Jimx.WebAggregator.DownloadDataApp");

        var mongoCitiesOpts = new MongoOptions(logger, "mongodb://localhost:27017", "demographics", "city-costs");
        var citiesConnection = new MongoConnection(mongoCitiesOpts).GetCollectionConnection<CityCostsItem>();

        var citiesFindTask = citiesConnection.Collection.FindAsync(_ => true);
        
        var mongoSalariesOpts = new MongoOptions(logger, "mongodb://localhost:27017", "demographics", "city-salaries");
        var salariesConnection = new MongoConnection(mongoSalariesOpts).GetCollectionConnection<CitySalary>();
        var salariesFindTask = salariesConnection.Collection.FindAsync(_ => true);
        
        var salaries = (await salariesFindTask).ToList();
        var cities = (await citiesFindTask).ToList();
        var citiesWithoutSalaries = cities.Select(c => c.Name).Except(salaries.Select(s => s.City));
        var salariesWithoutCities = salaries.Select(s => s.City).Except(cities.Select(c => c.Name));
        
        logger.LogInformation("citiesWithoutSalaries: {cities}", string.Join(", ", citiesWithoutSalaries));
        logger.LogInformation("salariesWithoutCities: {cities}", string.Join(", ", salariesWithoutCities));
        
        await Task.Delay(5000);
    }
}