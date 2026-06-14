using Jimx.Common.Helpers.Lists;
using Jimx.WebAggregator.Parser;
using Jimx.WebAggregator.Parser.Http;
using Jimx.WebAggregator.Persistent.MongoDB;
using Jimx.WebAggregator.Persistent.MongoDB.Operations;
using Jimx.WebAggregator.Domain.CityCosts;
using Microsoft.Extensions.Logging;
using Jimx.WebAggregator.DownloadDataApp.Options;
using Jimx.WebAggregator.Parser.Html.LifeLevel.ExtensionRequests;
using Jimx.WebAggregator.Parser.Html.LifeLevel.Models;


namespace Jimx.WebAggregator.DownloadDataApp;

public class LifeLevelParsingJob : IParsingJob
{
	private readonly ILogger _logger;
	private readonly ParsingWebSiteOptions _options;
	private readonly LifeLevelAdditionalData _additionalData;
	private readonly PersistencyOptionsBundle<LifeLevelPersistencyOptions> _persistencyOptions;
	

	public LifeLevelParsingJob(ILogger logger,
		ParsingWebSiteWithAdditionalData<LifeLevelAdditionalData> options,
		PersistencyOptionsBundle<LifeLevelPersistencyOptions> persistencyOptions)
	{
		_logger = logger;
		_options = options.Options;
		_additionalData = options.AdditionalData;
		_persistencyOptions = persistencyOptions;
	}

	public static string ConfigurationName => "LifeLevel";

	public async Task DoAsync()
	{
		var server = _persistencyOptions.Main.Server;
		var databaseName = _persistencyOptions.Subsection.DatabaseName;
		var costDictionaryCollection = _persistencyOptions.Subsection.CostDictionaryCollection;
		var costCollection = _persistencyOptions.Subsection.CostCollection;

		var mongoDictionaryOpts = new MongoOptions(_logger, server, databaseName, costDictionaryCollection);
		var mongoCitiesOpts = new MongoOptions(_logger, server, databaseName, costCollection);

		var baseUrl = _options.BaseUrl;
		
		var month = DateTime.UtcNow.Month;
		var year = DateTime.UtcNow.Year;
		
		var citiesToConsider = _additionalData.InitialCityRows
			.Select(r => new CityCostsItem(r.Values[0], r.Values[1], r.Values[2], month, year, []));
		
		var headers = HttpHeaders.CreateFromDictionary(_options.CookiesOptions.Headers?.ToDictionary(h => h.Name, h => h.Value));
		
		var citiesAsync = GetLifeLevelCitiesAsync(_logger, new Connection(_logger, baseUrl, headers, 10), citiesToConsider);
		var cities = await HandleCitiesAsync(_logger, citiesAsync, new MongoConnection(mongoCitiesOpts), new MongoConnection(mongoDictionaryOpts));
	}

	private async IAsyncEnumerable<CityCostsItem> GetLifeLevelCitiesAsync(ILogger logger, Connection connection, IEnumerable<CityCostsItem> cities)
	{
		var requestor = connection.GetRequestor();
		
		var commonCostRequest = new GeneralLifeLevelExtensionRequest(
			SettingsToRequestParameter(_additionalData.CommonCostRequestSettings));
		commonCostRequest.SetRequestor(requestor);

		var propertyInvestmentRequest = new GeneralLifeLevelExtensionRequest(
			SettingsToRequestParameter(_additionalData.PropertyInvestmentRequestSettings));
		propertyInvestmentRequest.SetRequestor(requestor);

		foreach (var city in cities)
		{
			using (logger.BeginScope("{city} requests", city.Name))
			{
				logger.LogInformation("Requests starting (0/2)");

				var cityWithCommonCostsRequest = commonCostRequest.Request(city);
				var cityWithPropertyInvestmentsRequest = propertyInvestmentRequest.Request(city);

				var cityDatas = await Task.WhenAll(cityWithCommonCostsRequest, cityWithPropertyInvestmentsRequest);

				var completeCity = new CityCostsItem(city.Name, city.Region, city.Country, city.Month, city.Year,
					cityDatas.Aggregate(city.DataItems, (items, newCity) => items.Union(newCity.DataItems)));

				yield return completeCity;

				logger.LogInformation("Requests finished (2/2)");
			}
		}
	}

	private async Task<IEnumerable<CityCostsItem>> HandleCitiesAsync(ILogger logger, IAsyncEnumerable<CityCostsItem> citiesAsync, 
		MongoConnection mongoCities, MongoConnection mongoDictionaries)
	{
		var dictionaryInsertOptions = new InsertOptions<CityDictionaryItem, string>(di => di.Value, key => di => di.Value == key, false);
		var cityUpsertOptions = new UpsertOptions<CityCostsItem, CityKey>(
			c => new CityKey(c.Name, c.Region, c.Country, c.Month, c.Year), 
			id => c => c.Name == id.Name && c.Region == id.Region && c.Country == id.Country && c.Month == id.Month && c.Year == id.Year, 
			null, 
			true);

		IList<CityCostsItem> cities = new List<CityCostsItem>();

		await foreach (var city in citiesAsync)
		{
			using (logger.BeginScope("{city} handle", city.Name))
			{
				logger.LogInformation("Handle started (0/2)");

				var dictionaryItems = city.DataItems.Select(di => new CityDictionaryItem(null, di.Key));

				var dictionaryInsertResult = await mongoDictionaries.DoWorkAsync(
					mongoDictionaries.GetCollectionConnection<CityDictionaryItem>(),
					new InsertMongoUnitOfWork<CityDictionaryItem, string>(dictionaryItems, dictionaryInsertOptions));

				logger.LogInformation("{CityName}: inserting dictionary result is {B}", 
					city.Name, !dictionaryInsertResult.IsFailure);

				var dictionaryItemsUpdated = dictionaryInsertResult.AllItems.ToArray();

				city.DataItems.Foreach(i =>
				{
					i.DictionaryId = dictionaryItemsUpdated.FirstOrDefault(di => di.Value == i.Key)?.Key ?? null;
				});
					
				var cityUpsertResult = await mongoCities.DoWorkAsync(
					mongoCities.GetCollectionConnection<CityCostsItem>(),
					new UpsertMongoUnitOfWork<CityCostsItem, CityKey>([city], cityUpsertOptions));

				logger.LogInformation("{CityName}: upsert city result is {B}", 
					city.Name, !cityUpsertResult.IsFailure);

				logger.LogInformation("Handle finished (2/2)");

				cities.Add(city);
			}
		}

		return cities;
	}

	private GeneralExtensionRequestSettings SettingsToRequestParameter(LifeLevelAdditionalDataRequestSettings settings)
	{
		return new GeneralExtensionRequestSettings(settings.DataSetName, settings.UrlTemplate); 
	}
}

public record CityKey(string Name, string Region, string Country, int Month, int Year);