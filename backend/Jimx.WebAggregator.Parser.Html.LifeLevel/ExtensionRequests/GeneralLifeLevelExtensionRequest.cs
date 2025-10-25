using Jimx.WebAggregator.Domain.CityCosts;
using Jimx.WebAggregator.Parser.Html.LifeLevel.Converters;
using Jimx.WebAggregator.Parser.Html.LifeLevel.Models;
using Jimx.WebAggregator.Parser.Http;

namespace Jimx.WebAggregator.Parser.Html.LifeLevel.ExtensionRequests;

public class GeneralLifeLevelExtensionRequest : LifeLevelExtensionRequest
{
    private readonly GeneralExtensionRequestSettings _settings;

    public GeneralLifeLevelExtensionRequest(GeneralExtensionRequestSettings settings)
    {
        _settings = settings;
    }

    protected override Uri GetUri(CityCostsItem input)
    {
        return new Uri(string.Format(_settings.UrlTemplate, input.Name));
    }

    protected override string GetDataSetName() => _settings.DataSetName;
    protected override HttpHeaders ProvideHeaders(CityCostsItem input) => new();


}