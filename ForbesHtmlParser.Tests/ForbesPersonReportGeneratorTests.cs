using HtmlAgilityPack;
using HtmlParser.Tests;

namespace ForbesHtmlParser.Tests;

internal class ForbesPersonReportGeneratorTests : PersonReportGeneratorTests
{

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_GetAge_Valid()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(rawHtml);

        var result = ParserTest_GetAge_Valid(52, new ForbesPersonReportGenerator(new MocLoggingService()), htmlDoc);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_GetAge_AgeNotPresent()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);
        rawHtml = rawHtml.Replace("Age", "this is not the correct string");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(rawHtml);

        var result = ParserTest_GetAge_Invalid( new ForbesPersonReportGenerator(new MocLoggingService()), htmlDoc);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_GetAge_AgeIsNotANumber()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);
        rawHtml = rawHtml.Replace("class=\"profile-stats__text\">52", "class=\"profile-stats__text\">not a number");

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(rawHtml);

        var result = ParserTest_GetAge_Invalid( new ForbesPersonReportGenerator(new MocLoggingService()), htmlDoc);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_GetCompanyName_Valid()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(rawHtml);

        var result = ParserTest_GetCompanyName_Valid("Tesla", new ForbesPersonReportGenerator(new MocLoggingService()), htmlDoc);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_LiXiaohuaAndFamily_GetCompanyName_NotPresent()
    {
        var liXiaohuaPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\Li Xiaohua and Family.html");
        var rawHtml = await File.ReadAllTextAsync(liXiaohuaPath);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(rawHtml);

        var result = ParserTest_GetCompanyName_Invalid(new ForbesPersonReportGenerator(new MocLoggingService()), htmlDoc);
        Assert.That(result, Is.True);
    }
}