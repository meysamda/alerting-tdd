namespace Alerting.Infrastructure.Data.Repositories.ContactPersons
{
    public class ContactPersonsFilter : IFilter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}