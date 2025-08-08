namespace Jimx.WebAggregator.Calculations;

public class UserTaxProfile
{
    private readonly string[] _ignoredParameters = ["taxtype"];
    
    public string[] Tags { get; }
    
    public UserFamily UserFamily { get; }

    public string[]? this[string tagKey]
    {
        get
        {
            var matchedTags = Tags.Select(ParseParameter).Where(v => v.Key == tagKey)
                .ToArray();
            
            if (matchedTags.Length > 1)
            {
                throw new Exception("Too much profile tags matched");
            }

            return matchedTags.Length == 0 ? null : matchedTags[0].Values;
        }
    }

    public UserTaxProfile(string[] tags, UserFamily userFamily)
    {
        Tags = [$"year:{DateTime.Today.Year}", ..tags];
        UserFamily = userFamily;
    }

    public bool IsParameterMatched(string taxParameterString)
    {
        var (taxParameterKey, taxParameterValues) = ParseParameter(taxParameterString);

        

        if (_ignoredParameters.Contains(taxParameterKey))
        {
            return true;
        }
        
        var currentProfileTagValues = this[taxParameterKey];
        
        if (currentProfileTagValues == null)
        {
            if (taxParameterValues.Contains("default"))
            {
                return true;
            }
            return false;
        }

        if (taxParameterKey == "taxpayer_age")
        {
            return IsParametersAsIntRangeMatched(currentProfileTagValues, taxParameterValues);
        }

        return IsParametersAsStringsMatched(currentProfileTagValues, taxParameterValues);
    }

    private bool IsParametersAsIntRangeMatched(string[] profileTagValues, string[] taxParameterValues)
    {
        var ageMargins = taxParameterValues.First().Split('-');
        if (ageMargins.Length != 2)
        {
            throw new ArgumentException("Invalid parameter string in tax parameters, should consists of 2 numbers and a hyphen in between", nameof(taxParameterValues));
        }

        if (profileTagValues.Length != 1)
        {
            throw new ArgumentException("Invalid parameter string in profile");
        }

        var taxpayerAge = int.Parse(profileTagValues[0]);
        var taxpayerAgeMin = int.Parse(ageMargins[0]);
        var taxpayerAgeMax = int.Parse(ageMargins[1]);

        return taxpayerAgeMin <= taxpayerAge && taxpayerAge <= taxpayerAgeMax;
    }

    private bool IsParametersAsStringsMatched(string[] profileTagValues, string[] taxParameterValues)
    {
        foreach (var taxParameterValue in taxParameterValues)
        {
            if (profileTagValues.Contains(taxParameterValue))
            {
                return true;
            }
        }

        return false;
    }

    private (string Key, string[] Values) ParseParameter(string parameterString)
    {
        var parameterKeyValue = parameterString.Split(':');
        if (parameterKeyValue.Length != 2)
        {
            throw new ArgumentException("Invalid parameter string", nameof(parameterString));
        }
        
        var parameterKey = parameterKeyValue[0].ToLower().Trim();
        var parameterValues = parameterKeyValue[1].Split(',')
            .Select(x => x.ToLower().Trim())
            .ToArray();
        
        return (parameterKey, parameterValues);
    }
}