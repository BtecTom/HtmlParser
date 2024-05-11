namespace Models;

public class Report
{
    /// <summary>
    /// The name of the subject of the report 
    /// </summary>
    public required string ReportSubjectName;

    /// <summary>
    /// The type of entity that the report is about
    /// </summary>
    public required string EntityType;

    /// <summary>
    /// object containing data about a person, if the entity type is "Person"
    /// </summary>
    public Person.Person? Person;
}