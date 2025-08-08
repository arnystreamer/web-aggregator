namespace Jimx.WebAggregator.API.Services;

public class CrossRatesService
{
    public decimal Get(string currencyCode)
    {
        switch (currencyCode)
        {
            case "USD": return 1.00m;
            case "CHF": return 1.243979m;
            case "EUR": return 1.158163m;
            case "KZT": return 0.0018432m;
            case "KGS": return 0.0114351m;
            case "RSD": return 0.009888m;
            case "PLN": return 0.271284m;
            case "DKK": return 0.155283m;
            case "GBP": return 1.328013m;
            case "AUD": return 0.646895m;
            case "THB": return 0.030920m;
            case "MYR": return 0.233781m;
            case "UAH": return 0.0370096m;
            case "NOK": return 0.097591m;
            case "SEK": return 0.103579m;
            case "ARS": return 0.000738m;
            case "ILS": return 0.292500m;
            case "RUB": return 0.012580m;
            case "CAD": return 0.726300m;
            case "AMD": return 0.002623m;
            case "TRY": return 0.024600m;
            case "EGP": return 0.020640m;
            case "IDR": return 0.000061095m;
            case "MXN": return 0.052920m;
            case "NZD": return 0.591200m;
            case "SGD": return 0.776700m;
            case "AED": return 0.272300m;
            case "VND": return 0.000038205m;
            case "CZK": return 0.0470947m;
            default:
                throw new ArgumentOutOfRangeException(nameof(currencyCode), currencyCode, "No cross rate found");
        }
    }
}