using HtmlAgilityPack;

namespace Jimx.WebAggregator.Parser.Html
{
	public class FuncTableFilter : ITableFilter
	{
		private readonly Func<HtmlNode, bool> _func;

		public FuncTableFilter(Func<HtmlNode, bool> func)
		{
			_func = func;
		}

		public bool Filter(HtmlNode tableNode)
		{
			return _func(tableNode);
		}

		public static FuncTableFilter Create(Func<HtmlNode, bool> func)
		{
			return new FuncTableFilter(func);
		}
	}
}
