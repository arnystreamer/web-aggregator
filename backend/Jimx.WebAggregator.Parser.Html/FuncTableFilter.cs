using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html;

public class FuncTableFilter : ITableFilter
{
	private readonly Func<HtmlNode, bool>? _func;

	public string Selector { get; init; }

	public FuncTableFilter(string selector, Func<HtmlNode, bool>? func)
	{
		Selector = selector;
		_func = func;
	}

	public FuncTableFilter(string selector)
		:this(selector, null)
	{
			
	}

	public bool Filter(HtmlNode tableNode)
	{
		return _func?.Invoke(tableNode) ?? true;
	}

	public static FuncTableFilter Create(string selector, Func<HtmlNode, bool>? func = null)
	{
		return new FuncTableFilter(selector, func);
	}
}