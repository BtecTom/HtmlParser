using HtmlParser.Tests;
using Models;
using Models.Person;

namespace ForbesHtmlParser.Tests;

internal class ForbesReportGeneratorTests : ReportGeneratorTests
{

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_Valid()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var result = await ParserTest_Valid(
            new Report {EntityType = nameof(Person), ReportSubjectName = "Elon Musk"},
            new ForbesReportGenerator(new MocLoggingService()), 
            elonMuskPath);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_SmokeTest_Valid()
    {
        var files = Directory.GetFiles(Path.Join(Environment.CurrentDirectory, "TestingResources")).ToList();
        foreach (var file in files)
        {
            Assert.That(await SmokeTest(new ForbesReportGenerator(new MocLoggingService()), file), Is.True);
        }
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_Invalid_DataProfileName_DoesNotExist()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);
        rawHtml = rawHtml.Replace("data-profile-name", "Wrong");

        var result = await ParserTest_UnableToGetReportSubjectsName(new ForbesReportGenerator(new MocLoggingService()), rawHtml);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_Invalid_DataProfileName_BadName()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);
        rawHtml = rawHtml.Replace("data-profile-name=\"Elon Musk\"\n", "data-profile-name=\"Bad Name\"\n");

        var result = await ParserTest_UnableToGetReportSubjectsName(new ForbesReportGenerator(new MocLoggingService()), rawHtml);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_Invalid_H1Value_NoH1()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);
        rawHtml = rawHtml.Replace("h1", "h2");

        var result = await ParserTest_UnableToGetReportSubjectsName(new ForbesReportGenerator(new MocLoggingService()), rawHtml);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_PersonalStats_DoesNotExist()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);
        rawHtml = rawHtml.Replace("Personal Stats", "Not a valid string");

        var result = await ParserTest_UnableToGetReportEntityType(new ForbesReportGenerator(new MocLoggingService()), rawHtml);
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task ForbesParserTest_ElonMusk_PersonalStats_CompanyStatsAlsoExists()
    {
        var elonMuskPath = Path.Join(Environment.CurrentDirectory, @"TestingResources\ElonMusk.html");
        var rawHtml = await File.ReadAllTextAsync(elonMuskPath);
        rawHtml = rawHtml.Replace("Personal Stats", "Not a valid string");

        var result = await ParserTest_UnableToGetReportEntityType(new ForbesReportGenerator(new MocLoggingService()), rawHtml);
        Assert.That(result, Is.True);
    }
}