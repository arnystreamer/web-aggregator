using Jimx.WebAggregator.Parser;
using Microsoft.Extensions.Configuration;

namespace Jimx.WebAggregator.DownloadDataApp.Options;

public static class ConfigurationHelper
{
    public static ParsingWebSite GetParsingWebsiteOptions<TParsingJob>(this IConfiguration config)
        where TParsingJob : IParsingJob
    {
        var parsingWebsites = config.GetSection("Parsing").Get<ParsingWebSites>();

        if (parsingWebsites == null)
        {
            throw new ApplicationException("ParsingWebSites is null");
        }
        
        var parsingWebsiteSection = parsingWebsites.Websites.FirstOrDefault(w => w.Key == TParsingJob.ConfigurationName);

        if (parsingWebsiteSection == null)
        {
            throw new ApplicationException($"ParsingWebSites with Key {TParsingJob.ConfigurationName} is null");
        }

        return parsingWebsiteSection;
    }
    
    public static ParsingWebSiteWithAdditionalData<TAdditionalData> GetParsingWebsiteOptionsWithAdditionalData<TParsingJob, TAdditionalData>(this IConfiguration config)
        where TParsingJob : IParsingJob
    {
        var parsingWebsiteSection = config
            .GetSection("Parsing")
            .GetSection("Websites")
            .GetChildren()
            .FirstOrDefault(w => w.GetValue("Key", String.Empty) == TParsingJob.ConfigurationName);

        if (parsingWebsiteSection == null)
        {
            throw new ApplicationException($"ParsingWebSites with Key {TParsingJob.ConfigurationName} is null");
        }

        var typedParsingWebsiteSection = parsingWebsiteSection.Get<ParsingWebSiteWithAdditionalData<TAdditionalData>>();

        if (typedParsingWebsiteSection == null)
        {
            throw new ApplicationException($"ParsingWebSites with Key {TParsingJob.ConfigurationName} is not of type {typeof(ParsingWebSiteWithAdditionalData<>).Name} of {typeof(TAdditionalData).Name}");
        }

        return typedParsingWebsiteSection;
    }

    public static PersistencyOptionsBundle<TSubsectionPersistencyOptions> GetPersistencyOptions<TSubsectionPersistencyOptions>(
        this IConfiguration config, 
        string subsectionName)
    {
        var persistencyOptions = config.GetSection("Persistency").Get<PersistencyOptions>();
        
        if (persistencyOptions == null)
        {
            throw new ApplicationException("PersistencyOptions is null");
        }
        
        var subsection = config.GetSection("Persistency").GetSection(subsectionName).Get<TSubsectionPersistencyOptions>();
        
        if (subsection == null)
        {
            throw new ApplicationException($"{typeof(TSubsectionPersistencyOptions).Name} in {subsectionName} is null");
        }
        
        return new PersistencyOptionsBundle<TSubsectionPersistencyOptions>(persistencyOptions, subsection);
    }
}