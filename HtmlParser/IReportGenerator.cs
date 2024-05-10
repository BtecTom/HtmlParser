using HtmlAgilityPack;
using HtmlParser.Custom_Exceptions;
using LoggingService;
using Models;

namespace HtmlParser
{
    /// <summary>
    /// Used to parse a HTML document containing data on a person or company
    /// </summary>
    public interface IReportGenerator
    {
        public ILoggingService LoggingService { get; }

        /// <summary>
        /// Generates a Json object containing a reports details when given a path to a scraped HTML page
        /// </summary>
        /// <param name="path">The path to a scraped HTML page</param>
        /// <returns>a Json object containing all the details of the page</returns>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs in getting a mandatory field</exception>
        public Task<string> GenerateJsonReport(string path);

        /// <summary>
        /// Generates a Report object when given a path to a scraped HTML page
        /// </summary>
        /// <param name="path">The path to a scraped HTML page</param>
        /// <returns>a Report object containing all the details of the page</returns>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs in getting a mandatory field</exception>
        public Task<Report> GenerateReportObjectFromPath(string path);

        /// <summary>
        /// Generates a Report object when given a string of HTML from a page
        /// </summary>
        /// <param name="htmlString">The string of HTML from a page</param>
        /// <returns>a Report object containing all the details of the page</returns>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs in getting a mandatory field</exception>
        public Task<Report> GenerateReportObjectFromHtmlString(string htmlString);

        /// <summary>
        /// Gets the value of the report subjects name from a Html document object
        /// </summary>
        /// <param name="htmlDoc">The Html object containing the HTML document data</param>
        /// <returns>the report subjects name</returns>
        string GetReportSubjectsName(HtmlDocument htmlDoc);

        /// <summary>
        /// Gets the value of the report subjects name from a Html document object
        /// </summary>
        /// <param name="htmlDoc">The Html object containing the HTML document data</param>
        /// <returns>The report entity type</returns>
        string GetEntityType(HtmlDocument htmlDoc);
    }
}
