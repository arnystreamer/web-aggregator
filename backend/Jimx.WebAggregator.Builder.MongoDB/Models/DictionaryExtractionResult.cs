namespace Jimx.WebAggregator.Builder.MongoDB.Models
{
	public record DictionaryExtractionResult<TItem, TDictionaryItem>(IEnumerable<TItem> Items, IEnumerable<TDictionaryItem> Dictionary);	
}
