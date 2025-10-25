namespace Jimx.WebAggregator.DownloadDataApp.Options;

public class PersistencyOptionsBundle<TSubsectionPersistencyOptions>(PersistencyOptions main, TSubsectionPersistencyOptions subsection)
{
    public PersistencyOptions Main { get; } = main;
    public TSubsectionPersistencyOptions Subsection { get; } = subsection;
}