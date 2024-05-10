using HtmlAgilityPack;
using HtmlParser;
using LoggingService;
using Models.Person;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ForbesHtmlParser
{
    public class ForbesPersonReportGenerator(ILoggingService loggingService)
        : DefaultPersonReportGenerator(loggingService)
    {

        private readonly char[] _currencySymbols = ['$', '€', '£', '¥', '₹'];

        #region Required Feilds

        public override int GetAge(HtmlDocument htmlDoc)
        {
            // get Age from personal stats
            var ageStr = GetPersonalStat(htmlDoc, "Age");

            // if age cant be found or isn't an int this means the format of the page my have changed
            if (ageStr == null || !int.TryParse(ageStr, out var age))
            {
                return base.GetAge(htmlDoc);
            }

            return age;
        }

        public override List<string> GetSourceOfWealth(HtmlDocument htmlDoc)
        {
            // get Source of Wealth from personal stats
            var sourceOfWealth = GetPersonalStat(htmlDoc, "Source of Wealth");

            // if this cant be found or isn't an int this means the format of the page my have changed
            return sourceOfWealth == null ? base.GetSourceOfWealth(htmlDoc) : sourceOfWealth.Split(',').Select(s => s.Trim()).ToList();
        }

        public override string GetResidence(HtmlDocument htmlDoc)
        {
            // get Residence from personal stats
            // if this cant be found or isn't an int this means the format of the page my have changed
            return GetPersonalStat(htmlDoc, "Residence") ?? base.GetResidence(htmlDoc);
        }

        public override string GetCitizenship(HtmlDocument htmlDoc)
        {
            // get Citizenship from personal stats
            // if this cant be found or isn't an int this means the format of the page my have changed
            return GetPersonalStat(htmlDoc, "Citizenship") ?? base.GetCitizenship(htmlDoc);
        }

        public override string GetEducation(HtmlDocument htmlDoc)
        {
            // get Education from personal stats
            // if this cant be found or isn't an int this means the format of the page my have changed
            return GetPersonalStat(htmlDoc, "Education") ?? base.GetEducation(htmlDoc);
        }

        public override List<Valuation> GetValuations(HtmlDocument htmlDoc)
        {
            var valuations = new List<Valuation>();
            
            // the valuations over tim are on the "networth chart"
            var netWorthChart = htmlDoc.DocumentNode.SelectNodes("//canvas[@class='person-networth-chart']");
            var dataChartDetails = netWorthChart?[0].Attributes["data-chart"];
            // if we cant find this chart, refer to the base for how to handle
            if (dataChartDetails == null) return base.GetValuations(htmlDoc);

            var dataPoints = JsonConvert.DeserializeObject<List<JObject>>(dataChartDetails.DeEntitizeValue);
            // if we cant find any data points, refer to the base for how to handle
            if (dataPoints == null) return base.GetValuations(htmlDoc);

            foreach (var dataPoint in dataPoints)
            {
                var date = dataPoint.GetValue("date")?.ToString();
                var worth = dataPoint.GetValue("worth")?.ToString();

                if (worth == null || date == null) return base.GetValuations(htmlDoc);

                // try and get the currency
                var currency = worth[0];
                worth = worth[1..];
                if (Array.IndexOf(_currencySymbols, currency) == -1)
                {
                    // if it's not in a currency we know, refer to teh base for hwo to handle
                    base.GetValuations(htmlDoc);
                }

                // convert the worth to a number
                var scalar = 1;
                var charToTrim = 'a';
                if (worth.Contains("b", StringComparison.CurrentCultureIgnoreCase))
                {
                    charToTrim = 'b';
                    scalar = 1000000000;

                }
                else if (worth.Contains("m", StringComparison.CurrentCultureIgnoreCase))
                {
                    scalar = 1000000;
                    charToTrim = 'm';
                }

                // if worth isnt a number then refer to base for how to handle this
                if (!double.TryParse(worth.ToLower().TrimEnd(charToTrim), out var worthValue))
                    return base.GetValuations(htmlDoc);

                worthValue *= scalar;

                valuations.Add(new Valuation {Date = date, Value = worthValue, Currency = currency.ToString()});
            }

            return valuations;
        }

        #endregion

        #region Non-required Feilds
        public override List<string>? GetUnstructuredData(HtmlDocument htmlDoc)
        {
            // get the bio content, there should only be of these, if not there may have been a change to the pages format
            var bioElements = htmlDoc.DocumentNode.SelectNodes("//ul[@class='listuser-content__bio--content']");
            if (bioElements is not { Count: 1 })
            {
                return base.GetUnstructuredData(htmlDoc);
            }

            // get all the li elements in teh bio
            var liElements =  bioElements[0].ChildNodes.Where(n => n.Name == "li");
            var unstructuredData = new List<string>();
            foreach (var liElement in liElements)
            {
                var text = liElement.InnerText;
                if (text == null)
                {
                    return base.GetUnstructuredData(htmlDoc);
                }
                unstructuredData.Add(text.Replace("\n                 ", ""));
            }

            return unstructuredData;
        }

        public override List<string>? GetRelatedParties(HtmlDocument htmlDoc)
        {
            // all related parties has the class connection__name so we can simply get those nodes and then return their string values
            var connectionNameElements = htmlDoc.DocumentNode.SelectNodes("//a[@class='connection__name']");

            if (connectionNameElements == null || connectionNameElements.Count == 0)
            {
                return base.GetRelatedParties(htmlDoc);
            }

            return connectionNameElements.ToList().Select(c => c.InnerText.Replace("&amp;", "and")).ToList();
        }

        public override string? GetMaritalStatus(HtmlDocument htmlDoc)
        {
            return GetPersonalStat(htmlDoc, "Marital Status") ?? base.GetMaritalStatus(htmlDoc);
        }

        public override List<InfluentialRankings>? GetInfluentialRankings(HtmlDocument htmlDoc)
        {
            // the rankings section is in the div with class of 'listuser-content__block ranking', there should only be one of these
            // if there is none, the subject may not be on any lists
            var rankingsElements = htmlDoc.DocumentNode.SelectNodes("//div[@class='listuser-content__block ranking']");
            if (rankingsElements is not {Count: 1})
            {
                return base.GetInfluentialRankings(htmlDoc);
            }

            // each ranking element is a div with 2 child div nodes
            // the first being the rank, the second being the list they are that rank in
            var rankingElements = rankingsElements[0].ChildNodes.Where(n =>
                n.Name == "div" &&
                n.ChildNodes.Count(c => c.Name == "div") == 2);

            var rankings = new List<InfluentialRankings>();

            foreach (var rankingElement in rankingElements)
            {
                // filter to just the div's
                var childDivs = rankingElement.ChildNodes.Where(n => n.Name == "div").ToList();

                // get the rank, and remove any #'s in the rank, the result should be a number
                // if not the format of the page has changed
                var position = childDivs[0].InnerText.Replace("#", "");
                if (!int.TryParse(position, out var positionNumber))
                {
                    base.GetInfluentialRankings(htmlDoc);
                    continue;
                }

                // the second div contains the List name and in brackets the year of the rank
                var listAndYear = childDivs[1].InnerText.Replace("\n", "").Replace(" ", "");
                var split = listAndYear.TrimEnd(')').Split("(");
                
                // if this splitting failed, the format of this field has changed
                if (split.Length != 2)
                {
                    base.GetInfluentialRankings(htmlDoc);
                    continue;
                }

                // the second element should be the year, try and parse this into a number
                // if this fails the format of the page has changed
                if (!int.TryParse(split[1].Trim(), out var year))
                {
                    base.GetInfluentialRankings(htmlDoc);
                    continue;
                }

                rankings.Add(new InfluentialRankings
                {
                    ListName = split[0].Trim(),
                    Position = positionNumber,
                    Year = year,
                    Ranker = "Forbes"
                });
            }

            return rankings;
        }

        public override string? GetCompanyName(HtmlDocument htmlDoc)
        {
            // the company name should be under the headline title div
            // if this is not present the subject may not have a company, or
            // if or there is more than 1 the format of the page my have changed
            var divElements = htmlDoc.DocumentNode.SelectNodes("//div[@class='listuser-header__headline--title']");
            if (divElements is not {Count: 1})
            {
                return base.GetCompanyName(htmlDoc);
            }

            // the first node of name 'a' should be the company name,
            // if one doesn't exist this probably means the format of the page has changed
            var companyNameNode = divElements[0].ChildNodes?.FirstOrDefault(n => n.Name == "a");
            return companyNameNode == null ? base.GetCompanyName(htmlDoc) : companyNameNode.InnerText;
        }

        public override string? GetPosition(HtmlDocument htmlDoc)
        {
            // the subjects position should be under the headline title div
            // if this is not present the subject may not have a position at a company, or
            // if or there is more than 1 the format of the page may have changed
            var divElements = htmlDoc.DocumentNode.SelectNodes("//div[@class='listuser-header__headline--title']");
            if (divElements is not {Count: 1})
            {
                return base.GetPosition(htmlDoc);
            }

            // the first node of name '#text' should be the subjects position,
            // if one doesn't exist this probably means the format of the page has changed
            var positionNode = divElements[0].ChildNodes.FirstOrDefault(n => n.Name == "#text");
            return positionNode == null ? base.GetCompanyName(htmlDoc) :
                // the position ends in a "," as on the screen this is followed by the company, this comman isn't needed
                positionNode.InnerText.Trim().TrimEnd(',');
        }

        public override int? GetChildren(HtmlDocument htmlDoc)
        {
            // get the number of children from personal stats
            var childrenStr = GetPersonalStat(htmlDoc, "Children");
            // this should be a number, if it's not then the formant has changed
            // if its null then maybe this person has none
            // call the base for how to handle either
            if (childrenStr == null || !int.TryParse(childrenStr, out var children))
            {
                return base.GetChildren(htmlDoc);
            }

            return children;
        }

        private string? GetPersonalStat(HtmlDocument htmlDoc, string elementName)
        {
            // personal stats are in a dl node with class 'listuser-block'
            var personalStatElements = htmlDoc.DocumentNode.SelectNodes("//dl[@class='listuser-block']");
            // if this cant be found or there are multiple, log this issue as it likely means the format has changed
            if (personalStatElements is not {Count: 1})
            {
                LoggingService.LogWarning($"invalid result from getting nodes with an xpath of '//dl[@class='listuser-block']', {personalStatElements?.Count} nodes found");
                return null;
            }

            // Go through all the personal stats child node;
            // Foreach child node, check its child nodes and see if any of them have the
            // inter next matching the stat we are looking for .
            // If there is a match, that is the stat element we are looking for,
            // get its child Span as they have the details we need.
            var element = personalStatElements[0].ChildNodes.FirstOrDefault(d =>
                d.ChildNodes.FirstOrDefault(n =>
                    n.InnerText == elementName) != null)?.ChildNodes
                .FirstOrDefault(c => c.Name == "dd")?.ChildNodes
                .FirstOrDefault(c => c.Name == "span");

            // if either no elements could be found or not 2 where
            // this likely means either the stat doesn't exist or the format of the page has changed
            if (element == null)
            {
                LoggingService.LogWarning($"invalid result from getting personal status nodes with a name of '{elementName}'");
                return null;
            }

            return element.InnerText.Replace("\n                     ", "");
        }

        #endregion
    }
}
