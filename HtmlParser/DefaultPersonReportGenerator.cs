using HtmlAgilityPack;
using HtmlParser.Custom_Exceptions;
using LoggingService;
using Models.Person;

namespace HtmlParser;

/// <summary>
/// The default implementation of the IPersonReportGenerator interface. By default, it;
///     assumes every optional field is expected,
///     throws an <exception cref="MandatoryFieldNotFoundException">exception</exception> for all functions getting any mandatory fields,
///     logs a not found warning for all functions getting any non-mandatory fields
/// </summary>
public abstract class DefaultPersonReportGenerator(ILoggingService loggingService) : IPersonReportGenerator
{
    public ILoggingService LoggingService { get; } = loggingService;


    // for the default generator, set every field to be expected on the page
    public virtual PersonReportGeneratorConfiguration PersonReportGeneratorConfiguration { get; } = new()
    {
        ChildrenIsExpected = true,
        CompanyNameIsExpected = true,
        CompanyPositionIsExpected = true,
        InfluentialRankingsIsExpected = true,
        MaritalStatusIsExpected = true,
        RelatedPartiesIsExpected = true,
        UnstructuredDataIsExpected = true
    };

    public Task<Person> GeneratePersonFromHtmlDoc(HtmlDocument htmlDoc)
    {
        return Task.FromResult(new Person
        {
            Age = GetAge(htmlDoc),
            SourceOfWealth = GetSourceOfWealth(htmlDoc),
            Residence = GetResidence(htmlDoc),
            Citizenship = GetCitizenship(htmlDoc),
            Education = GetEducation(htmlDoc),
            Valuations = GetValuations(htmlDoc),

            Children = PersonReportGeneratorConfiguration.ChildrenIsExpected ? GetChildren(htmlDoc): null,
            CompanyName = PersonReportGeneratorConfiguration.CompanyNameIsExpected ? GetCompanyName(htmlDoc): null,
            InfluentialRankings = PersonReportGeneratorConfiguration.InfluentialRankingsIsExpected? GetInfluentialRankings(htmlDoc): null,
            MaritalStatus = PersonReportGeneratorConfiguration.MaritalStatusIsExpected? GetMaritalStatus(htmlDoc): null,
            CompanyPosition = PersonReportGeneratorConfiguration.CompanyPositionIsExpected? GetPosition(htmlDoc): null,
            RelatedParties = PersonReportGeneratorConfiguration.RelatedPartiesIsExpected? GetRelatedParties(htmlDoc): null,
            UnstructuredData = PersonReportGeneratorConfiguration.UnstructuredDataIsExpected? GetUnstructuredData(htmlDoc): null
        });
    }

    #region Required Feilds

    public virtual int GetAge(HtmlDocument htmlDoc)
    {
        return (int)HandleMandatoryFieldNotFound();
    }

    public virtual List<string> GetSourceOfWealth(HtmlDocument htmlDoc)
    {
        return (List<string>)HandleMandatoryFieldNotFound();
    }

    public virtual string GetResidence(HtmlDocument htmlDoc)
    {
        return (string)HandleMandatoryFieldNotFound();
    }

    public virtual string GetCitizenship(HtmlDocument htmlDoc)
    {
        return (string)HandleMandatoryFieldNotFound();
    }

    public virtual string GetEducation(HtmlDocument htmlDoc)
    {
        return (string)HandleMandatoryFieldNotFound();
    }

    public virtual List<Valuation> GetValuations(HtmlDocument htmlDoc)
    {
        return (List<Valuation>)HandleMandatoryFieldNotFound();
    }

    /// <summary>
    /// How to handle any mandatory fields that are not found - always throws an exception
    /// </summary>
    /// <exception cref="MandatoryFieldNotFoundException"></exception>
    private object HandleMandatoryFieldNotFound()
    {
        // todo: add better error handling here, maybe try and work out a why to get the property it was trying to get the value of?
        LoggingService.LogError("The Value of a required field could not be found in data set");
        throw new MandatoryFieldNotFoundException("The Value of a required field could not be found in data set");
    }
    #endregion

    #region Non-required Feilds

    public virtual List<string>? GetUnstructuredData(HtmlDocument htmlDoc)
    {
        return (List<string>?)HandleNonMandatoryFieldNotFound();
    }

    public virtual List<string>? GetRelatedParties(HtmlDocument htmlDoc)
    {
        return (List<string>?)HandleNonMandatoryFieldNotFound();
    }

    public virtual string? GetMaritalStatus(HtmlDocument htmlDoc)
    {
        return (string?)HandleNonMandatoryFieldNotFound();
    }

    public virtual List<InfluentialRankings>? GetInfluentialRankings(HtmlDocument htmlDoc)
    {
        return (List<InfluentialRankings>?)HandleNonMandatoryFieldNotFound();
    }

    public virtual string? GetCompanyName(HtmlDocument htmlDoc)
    {
        return (string?)HandleNonMandatoryFieldNotFound();
    }

    public virtual string? GetPosition(HtmlDocument htmlDoc)
    {
        return (string?)HandleNonMandatoryFieldNotFound();
    }

    public virtual int? GetChildren(HtmlDocument htmlDoc)
    {
        return (int?)HandleNonMandatoryFieldNotFound();
    }

    /// <summary>
    /// how to handle any non-mandatory fields that are not found
    /// </summary>
    /// <returns>Returns null after logging the issue</returns>
    private object? HandleNonMandatoryFieldNotFound()
    {
        // todo: add better error handling here, maybe try and work out a why to get the property it was trying to get the value of?
        LoggingService.LogWarning("The value of an optional field could not be found in data set");
        return null;
    }

    #endregion

}