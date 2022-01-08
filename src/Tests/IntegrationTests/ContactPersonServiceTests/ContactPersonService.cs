using System;
using Alerting.Infrastructure.Data.DbContexts;
using Xunit;

namespace Alerting.Tests.IntegrationTests.ContactPersonService
{
    public partial class ContactPersonService
    {
        [Fact]
        public void Create_valid_contact_person()
        {
            var dbContext = new AlertingDbContext();
            var repository = new ContactPersonRepository(dbContext);
            var contactPerson = new ContactPerson();
            var sut = new ContactPersonService(repository);

            var result = sut.CreateContactPerson(contactPerson);

            result.Should().BeGreaterThan(0);
        }
    }
}
