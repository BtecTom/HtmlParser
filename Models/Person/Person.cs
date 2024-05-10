namespace Models.Person
{
    public class Person
    {
        public required int Age;
        public required string Residence;
        public required string Citizenship;
        public required string Education;
        public required List<string> SourceOfWealth;
        public required List<Valuation> Valuations;

        public string? CompanyPosition;
        public string? CompanyName;
        public int? Children;
        public string? MaritalStatus;
        public List<string>? UnstructuredData;
        public List<string>? RelatedParties;
        public List<InfluentialRankings>? InfluentialRankings;
    }
}
