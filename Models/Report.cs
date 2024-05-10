namespace Models
{
    public class Report
    {
        public required string ReportSubjectName;
        public required string EntityType;

        public Person.Person? Person;
    }
}
