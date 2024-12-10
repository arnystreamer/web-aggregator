using Jimx.WebAggregator.Parser.Constructor;

namespace Jimx.WebAggregator.Parser.Helpers
{
    public static class PopulatableHelper
	{
		public static IBuilder<TOutput> PopulateByRequest<TPopulationRequest, TInput, TOutput>(this IBuilder<TInput> populatable, TPopulationRequest populationRequest)
			where TPopulationRequest : PopulationRequest<TInput, TOutput>
		{
			populationRequest.SetRequestor(populatable.Requestor);

			return new SimpleBuilder<TOutput>(
				populatable.Requestor,
				new Lazy<TOutput>(() => populationRequest.Request(populatable.ExecutingFactory.Value)));
		}

		public static IBuilder<IEnumerable<TOutput>> PopulateByRequest<TPopulationRequest, TInput, TOutput>(this IBuilder<IEnumerable<TInput>> populatable, TPopulationRequest populationRequest)
			where TPopulationRequest : PopulationRequest<TInput, TOutput>
		{
			populationRequest.SetRequestor(populatable.Requestor);

			return new SimpleBuilder<IEnumerable<TOutput>>(
				populatable.Requestor,
				new Lazy<IEnumerable<TOutput>>(() => populatable.ExecutingFactory.Value.Select(v => populationRequest.Request(v))));
		}
	}
}
