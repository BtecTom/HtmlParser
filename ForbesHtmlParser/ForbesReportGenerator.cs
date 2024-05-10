using HtmlAgilityPack;
using HtmlParser;
using LoggingService;
using Models;
using Models.Person;

namespace ForbesHtmlParser
{
    public class ForbesReportGenerator(ILoggingService loggingService) : DefaultReportGenerator(loggingService)
    {
        /// <summary>
        /// Default implementation for generating a Report object when given a string of HTML from a page is not implemented
        /// </summary>
        /// <param name="htmlString">The string of HTML from a page</param>
        /// <exception cref="NotImplementedException">Thrown when called</exception>
        public override async Task<Report> GenerateReportObjectFromHtmlString(string htmlString)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);

            var report = new Report { ReportSubjectName = GetReportSubjectsName(htmlDoc), EntityType = GetEntityType(htmlDoc) };

            switch (report.EntityType)
            {
                case nameof(Person):
                    report.Person = await new ForbesPersonReportGenerator(loggingService).GeneratePersonFromHtmlDoc(htmlDoc);
                    break;
            }

            return report;
        }

        public override string GetReportSubjectsName(HtmlDocument htmlDoc)
        {
            // ReSharper disable once InvalidXmlDocComment
            /// The Name of the subject of the report is in the H1 element.
            /// There is only 1 H1 element at this time, the value of the element is the name of the subject,
            /// but so is the "data-profile-name" Attribute; these should be equal.
            /// In case the page changes, check both values exist and then make sure they equal.
            /// if any issues arise doing this, reffer to the default implementation for how to handle this.
            var h1Elements = htmlDoc.DocumentNode.SelectNodes("//h1");
            var nameElement = h1Elements?.FirstOrDefault(h =>
                h.Attributes.FirstOrDefault(a =>
                    a.Name == "data-profile-name") != null);

            if (nameElement != null)
            {
                var cleanInnerText = nameElement.InnerText.Clean();

                if (cleanInnerText ==
                    nameElement.Attributes.FirstOrDefault(a => a.Name == "data-profile-name")?.Value.Clean())
                {
                    return cleanInnerText;
                }
            }

            return base.GetReportSubjectsName(htmlDoc);
        }

        public override string GetEntityType(HtmlDocument htmlDoc)
        {
            // ReSharper disable once InvalidXmlDocComment
            /// if the Page is for a person, it will contain the string "Personal Stats" but should not contain "Company Stats".
            /// If this isnt the case then the page is not one this parser can handle so refer to the default implementation for how to handle this
            if (htmlDoc.ParsedText.Contains("Personal Stats") && !htmlDoc.ParsedText.Contains("Company Stats"))
            {
                return nameof(Person);
            }

            return base.GetEntityType(htmlDoc);
        }
    }
}
