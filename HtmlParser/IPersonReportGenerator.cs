using HtmlAgilityPack;
using HtmlParser.Custom_Exceptions;
using LoggingService;
using Models.Person;

namespace HtmlParser
{
    /// <summary>
    /// Used to parse a HTML document containing data on a person
    /// </summary>
    public interface IPersonReportGenerator
    {
        public ILoggingService LoggingService { get; }

        /// <summary>
        /// Contains configuration data for if a Person Report Generator should expect to be seeing optional fields.
        /// This is used to trigger the error handling for expected but non-present optional fields
        /// </summary>
        PersonReportGeneratorConfiguration PersonReportGeneratorConfiguration { get; }

        /// <summary>
        /// Generates a person object from the details in a HTML document 
        /// </summary>
        /// <param name="htmlDoc">The scraped HTML data from the web page</param>
        /// <returns>A person object with all details</returns>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs in getting a mandatory field</exception>
        Task<Person> GeneratePersonFromHtmlDoc(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the persons age
        /// </summary>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs Getting this field</exception>
        int GetAge(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the persons Sources of Wealth
        /// </summary>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs Getting this field</exception>
        List<string> GetSourceOfWealth(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the persons Residence
        /// </summary>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs Getting this field</exception>
        string GetResidence(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the persons Citizenship
        /// </summary>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs Getting this field</exception>
        string GetCitizenship(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the persons Education
        /// </summary>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs Getting this field</exception>
        string GetEducation(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the persons Valuations
        /// </summary>
        /// <exception cref="MandatoryFieldNotFoundException">Thrown if an error occurs Getting this field</exception>
        List<Valuation> GetValuations(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the Unstructured data about the person
        /// </summary>
        /// <returns>Null if none could be found</returns>
        List<string>? GetUnstructuredData(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the Related Parties to the person
        /// </summary>
        /// <returns>Null if none could be found</returns>
        List<string>? GetRelatedParties(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the Marital Status of the person
        /// </summary>
        /// <returns>Null if none could be found</returns>
        string? GetMaritalStatus(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the Influential Rankings of the person
        /// </summary>
        /// <returns>Null if none could be found</returns>
        List<InfluentialRankings>? GetInfluentialRankings(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the Company Name where the person works
        /// </summary>
        /// <returns>Null if none could be found</returns>
        string? GetCompanyName(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the Position of the person at the company they work at
        /// </summary>
        /// <returns>Null if none could be found</returns>
        string? GetPosition(HtmlDocument htmlDoc);

        /// <summary>
        /// Get the number of Children the person has
        /// </summary>
        /// <returns>Null if none could be found</returns>
        int? GetChildren(HtmlDocument htmlDoc);
    }
}
