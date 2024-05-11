using HtmlAgilityPack;
using HtmlParser.Custom_Exceptions;
using NUnit.Framework;

namespace HtmlParser.Tests;

public class PersonReportGeneratorTests
{
    protected bool ParserTest_GetAge_Valid(int expectedResult, IPersonReportGenerator reportGenerator, HtmlDocument htmlDoc)
    {
        var result = reportGenerator.GetAge(htmlDoc);
        Assert.That(result, Is.EqualTo(expectedResult));
        return true;
    }

    protected bool ParserTest_GetAge_Invalid(IPersonReportGenerator reportGenerator, HtmlDocument htmlDoc)
    {
        Assert.Multiple(() =>
        {
            Assert.Throws<MandatoryFieldNotFoundException>(
                () => reportGenerator.GetAge(htmlDoc),
                "The Value of a required field could not be found in data set");
            Assert.IsTrue(reportGenerator.LoggingService.GetErrorLogs().Contains("The Value of a required field could not be found in data set"));
        });
        return true;
    }

    protected bool ParserTest_GetCompanyName_Valid(string expectedResult, IPersonReportGenerator reportGenerator, HtmlDocument htmlDoc)
    {
        var result = reportGenerator.GetCompanyName(htmlDoc);

        Assert.That(result, Is.EqualTo(expectedResult));

        return true;
    }

    protected bool ParserTest_GetCompanyName_Invalid(IPersonReportGenerator reportGenerator, HtmlDocument htmlDoc)
    {
        var result = reportGenerator.GetCompanyName(htmlDoc);
            
        Assert.IsTrue(reportGenerator.LoggingService.GetWarningLogs().Contains("The value of an optional field could not be found in data set"));
        Assert.That(result, Is.EqualTo(null));

        return true;
    }
}