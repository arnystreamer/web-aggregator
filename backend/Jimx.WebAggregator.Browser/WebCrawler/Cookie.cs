namespace Jimx.WebAggregator.Browser.WebCrawler;

public record Cookie(string Name, string Value, string Domain, string Path, float ExpiresInMs);

public record CookieDiff(
    string Name,
    bool IsChanged,
    Cookie? OldValue,
    Cookie? NewValue);