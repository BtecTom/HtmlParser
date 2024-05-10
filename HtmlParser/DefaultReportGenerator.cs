using HtmlAgilityPack;
using HtmlParser.Custom_Exceptions;
using LoggingService;
using Models;
using SchemaManager;

namespace HtmlParser
{
    /// <summary>
    /// The default implementation for a report generator
    /// </summary>
    public abstract class DefaultReportGenerator(ILoggingService loggingService) : IReportGenerator
    {
        public ILoggingService LoggingService { get; } = loggingService;

        /// <summary>
        /// Default implementation for generating a JSON report  when given a path to a scraped HTML page 
        /// </summary>
        /// <param name="path">The path to a scraped HTML page</param>
        /// <returns>a Json object containing all the details of the page</returns>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs in getting a mandatory field</exception>
        public virtual async Task<string> GenerateJsonReport(string path)
        {
            var report = await GenerateReportObjectFromPath(await File.ReadAllTextAsync(path));
            return Schema.ReportToJson(report);
        }

        /// <summary>
        /// Default implementation for generating a report object when given a path to a scraped HTML page
        /// </summary>
        /// <param name="path">The path to a scraped HTML page</param>
        /// <returns>a Report object containing all the details of the page</returns>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs in getting a mandatory field</exception>
        public virtual async Task<Report> GenerateReportObjectFromPath(string path)
        {
            return await GenerateReportObjectFromHtmlString(await File.ReadAllTextAsync(path));
        }

        /// <summary>
        /// Default implementation for generating a Report object when given a string of HTML from a page is not implemented
        /// </summary>
        /// <param name="htmlString">The string of HTML from a page</param>
        /// <exception cref="NotImplementedException">Thrown when called</exception>
        public virtual async Task<Report> GenerateReportObjectFromHtmlString(string htmlString)
        {
            await Task.CompletedTask;

            throw new NotImplementedException(
                "There is no default implementation of how to parse a HTML page into a report");
        }

        public virtual string GetReportSubjectsName(HtmlDocument htmlDoc)
        {
            // todo: add better error handling here
            loggingService.LogError("The Value of a required field could not be found in data set");
            throw new MandatoryFieldNotFoundException("The Value of a required field could not be found in data set");
        }

        public virtual string GetEntityType(HtmlDocument htmlDoc)
        {
            // todo: add better error handling here
            loggingService.LogError("The Value of a required field could not be found in data set");
            throw new MandatoryFieldNotFoundException("The Value of a required field could not be found in data set");
        }
    }
}
