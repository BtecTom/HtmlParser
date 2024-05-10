using HtmlParser.Custom_Exceptions;
using Models;
using NUnit.Framework;

namespace HtmlParser.Tests
{
    public class ReportGeneratorTests
    {
        protected async Task<bool> ParserTest_Valid(Report expectedResults, IReportGenerator reportGenerator, string testDataPath )
        {
            var result = await reportGenerator.GenerateReportObjectFromPath(testDataPath);

            Assert.Multiple(() =>
            {
                Assert.That(result.ReportSubjectName, Is.EqualTo(expectedResults.ReportSubjectName));
                Assert.That(result.EntityType, Is.EqualTo(expectedResults.EntityType));
            });

            return true;
        }

        protected async Task<bool> SmokeTest(IReportGenerator reportGenerator, string testDataPath)
        {
            await reportGenerator.GenerateReportObjectFromPath(testDataPath);

            Assert.Multiple(() =>
            {
                Assert.IsTrue(reportGenerator.LoggingService.GetErrorLogs().Count == 0);
            });

            return true;
        }

        public async Task<bool> ParserTest_UnableToGetReportSubjectsName(IReportGenerator reportGenerator, string rawHtml)
        {
            Assert.ThrowsAsync<MandatoryFieldNotFoundException>(
                async Task () => await reportGenerator.GenerateReportObjectFromHtmlString(rawHtml),
                "Unable to get Report Subjects Name");
            Assert.IsTrue(reportGenerator.LoggingService.GetErrorLogs().Contains("The Value of a required field could not be found in data set"));

            return true;
        }

        public async Task<bool> ParserTest_UnableToGetReportEntityType(IReportGenerator reportGenerator, string rawHtml)
        {
            Assert.ThrowsAsync<MandatoryFieldNotFoundException>(
                async Task () => await reportGenerator.GenerateReportObjectFromHtmlString(rawHtml),
                "Unable to get Reports Entity Type");
            Assert.IsTrue(reportGenerator.LoggingService.GetErrorLogs().Contains("The Value of a required field could not be found in data set"));
            return true;
        }
    }
}
