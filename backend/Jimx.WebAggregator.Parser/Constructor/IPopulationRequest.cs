namespace Jimx.WebAggregator.Parser.Constructor
{
    public interface IPopulationRequest<TInput, TOutput>
    {
        TOutput Request(TInput input);
    }
}
