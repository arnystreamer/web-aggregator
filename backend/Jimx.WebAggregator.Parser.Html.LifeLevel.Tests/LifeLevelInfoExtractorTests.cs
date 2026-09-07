using Jimx.WebAggregator.Parser.Html.LifeLevel.Converters;

namespace Jimx.WebAggregator.Parser.Html.LifeLevel.Tests;

public class LifeLevelInfoExtractorTests
{
    private LifeLevelInfoExtractor _extractor;
    
    [SetUp]
    public void Setup()
    {
        _extractor = new LifeLevelInfoExtractor();
    }

    [Test]
    public void Test1()
    {
        var html = GetEmbeddedResourceFileContents("test1.html");
        var dataItems = _extractor.Extract(html, "City common costs");
        
        Assert.That(dataItems, Is.Not.Null);
        Assert.That(dataItems.Count(di => di.Value == null), Is.Zero);
        
        Assert.Pass();
    }
    
    [Test]
    public void Test2()
    {
        var html = GetEmbeddedResourceFileContents("test2.html");
        var dataItems = _extractor.Extract(html, "City property investment");
        
        Assert.That(dataItems, Is.Not.Null);
        Assert.That(dataItems.Count(di => di.Value == null), Is.Zero);
        
        Assert.Pass();
    }
    
    [Test]
    public void Test3()
    {
        var html = GetEmbeddedResourceFileContents("test3.html");
        var dataItems = _extractor.Extract(html, "City common costs");
        
        Assert.That(dataItems, Is.Not.Null);
        Assert.That(dataItems.Count(di => di.Value == null), Is.Zero);
        
        Assert.Pass();
    }
    
    [Test]
    public void Test4()
    {
        var html = GetEmbeddedResourceFileContents("test4.html");
        var dataItems = _extractor.Extract(html, "City common costs");
        
        Assert.That(dataItems, Is.Not.Null);
        Assert.That(dataItems.Count(di => di.Value == null), Is.Zero);
        
        Assert.Pass();
    }
    
    [Test]
    public void Test5()
    {
        var html = GetEmbeddedResourceFileContents("test5.html");
        var dataItems = _extractor.Extract(html, "City common costs");
        
        Assert.That(dataItems, Is.Not.Null);
        
        Assert.Pass();
    }
    
    [Test]
    public void Test6()
    {
        var html = GetEmbeddedResourceFileContents("test6.html");
        var dataItems = _extractor.Extract(html, "City common costs");
        
        Assert.That(dataItems, Is.Not.Null);
        Assert.That(dataItems.Count(di => di.Value == null), Is.Zero);
        
        Assert.Pass();
    }
    
    [Test]
    public void Test7()
    {
        var html = GetEmbeddedResourceFileContents("test7.html");
        var dataItems = _extractor.Extract(html, "City common costs");
        
        Assert.That(dataItems, Is.Not.Null);
        Assert.That(dataItems.Count(di => di.Value == null), Is.Zero);
        
        Assert.Pass();
    }
    
    [Test]
    public void Test8()
    {
        var html = GetEmbeddedResourceFileContents("test8.html");
        var dataItems = _extractor.Extract(html, "City common costs");
        
        Assert.That(dataItems, Is.Not.Null);
        Assert.That(dataItems.Count(di => di.Value == null), Is.Zero);
        
        Assert.Pass();
    }
    
    [Test]
    public void Test9()
    {
        var html = GetEmbeddedResourceFileContents("test9.html");
        var dataItems = _extractor.Extract(html, "City common costs");
        
        Assert.That(dataItems, Is.Not.Null);
        Assert.That(dataItems.Count(di => di.Value == null), Is.Zero);
        
        Assert.Pass();
    }

    private string GetEmbeddedResourceFileContents(string resourceFileName)
    {
        var assembly = typeof(LifeLevelInfoExtractorTests).Assembly;
        string resourceName = $"Jimx.WebAggregator.Parser.Html.LifeLevel.Tests.ExtractorTestFiles.{resourceFileName}";
        using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"Could not find embedded resource: {resourceName}");
        using var reader = new StreamReader(stream);
        
        return reader.ReadToEnd();
    }
}