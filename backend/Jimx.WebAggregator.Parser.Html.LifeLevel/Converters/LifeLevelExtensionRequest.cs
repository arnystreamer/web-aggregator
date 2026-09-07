using Jimx.WebAggregator.Domain.CityCosts;
using Jimx.WebAggregator.Parser.Builder;

namespace Jimx.WebAggregator.Parser.Html.LifeLevel.Converters;

public abstract class LifeLevelExtensionRequest : ExtensionRequest<CityCostsItem, CityCostsItem>
{
	protected override async Task<CityCostsItem> GetInformationFromResponse(CityCostsItem input, HttpResponseMessage message)
	{
		var html = await message.Content.ReadAsStringAsync();
		
		var newDataItems = new LifeLevelInfoExtractor().Extract(html, GetDataSetName());

		return new CityCostsItem(input.Name, input.Region, input.Country, input.Year, input.Month, input.DataItems.Union(newDataItems));
	}

	protected abstract string GetDataSetName();
}