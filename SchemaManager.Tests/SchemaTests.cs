using Models;
using Models.Person;
using Newtonsoft.Json.Schema;

namespace SchemaManager.Tests;

public class GenerateSchemaTest
{
    private readonly JSchema _schema = Schema.GetLatestSchema();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GenerateSchemaTest_Pass()
    {
        var report = new Report
        {
            ReportSubjectName = "test", 
            EntityType = nameof(Person),
            Person = new Person
            {
                Age = 20,
                Citizenship = "UK",
                Education = "none",
                MaritalStatus = "Single",
                Residence = "Bedford",
                SourceOfWealth = ["self-made"],
                Valuations = [new Valuation{Date = "test", Value = 100, Currency = "$"}]
            }
        };
        var json = Schema.ReportToJson(report);

        Assert.That(Schema.Validate(_schema, json), Is.True);
    }

    [Test]
    public void GenerateSchemaTest_Fail()
    {
        var report = new Report
        {
            ReportSubjectName = "test",
            EntityType = nameof(Person),
            Person = new Person
            {
                Age = 20,
                Citizenship = "UK",
                Education = "none",
                MaritalStatus = "Single",
                Residence = "Bedford",
                SourceOfWealth = ["self-made"],
                Valuations = [new Valuation{Date = "test", Value = 100, Currency = "$"}]
            }
        };
        var json = Schema.ReportToJson(report);

        json = json.Replace("Age", "InvalidProperty");

        Assert.That(Schema.Validate(_schema, json), Is.False);
    }
}