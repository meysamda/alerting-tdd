namespace Alerting.Tests.AcceptanceTests.ContactPersons
{
    public class GetContactPersonsPagableFilter : GetContactPersonsFilter
    {
        public int? Skip { get; set; }
        public int? Limit { get; set; }
        public string Sort { get; set; }
    }
}