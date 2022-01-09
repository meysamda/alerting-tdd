using System;
using Alerting.Application.Services;
using Alerting.Infrastructure.Data.DbContexts.Entities;
using Alerting.Infrastructure.Data.Repositories;
using Alerting.Tests.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Alerting.Tests.IntegrationTests.ContactPersonServiceTests
{
    [Collection("No Parallel")]
    public partial class ContactPersonServiceTests : IClassFixture<AlertingDbContextFixture>
    {
        private readonly AlertingDbContextFixture _dbFixture;

        public ContactPersonServiceTests(AlertingDbContextFixture fixture)
        {
            _dbFixture = fixture;
        }

        [Fact]
        public void Create_valid_contact_person()
        {
            // arrange
            using (var context = _dbFixture.GetDbContext())
            {
                context.ClearData();
            }

            var contactPerson = new ContactPerson();
            int id;
            using (var context = _dbFixture.GetDbContext())
            {
                var repository = new ContactPersonRepository(context);
                var sut = new ContactPersonService(repository);

                // act
                id = sut.CreateContactPerson(contactPerson);
            }

            // assert
            id.Should().BeGreaterThan(0);
            using (var context = _dbFixture.GetDbContext())
            {
                var repository = new ContactPersonRepository(context);
                var contactPersonFromDb = repository.GetContactPersonById(id);
                contactPersonFromDb.Should().Equals(contactPerson);
            }
        }
    }
}
