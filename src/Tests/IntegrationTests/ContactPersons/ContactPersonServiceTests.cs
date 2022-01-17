using Alerting.Application.ContactPersons;
using Alerting.Infrastructure.Data.DbContexts.Entities;
using Alerting.Infrastructure.Data.Repositories.ContactPersons;
using Alerting.Tests.IntegrationTests.Common;
using AutoMapper;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Alerting.Tests.IntegrationTests.ContactPersons
{
    [Collection("No Parallel")]
    public partial class ContactPersonServiceTests : IClassFixture<AlertingDbContextFixture>, IClassFixture<AutoMapperFixture>
    {
        private readonly AlertingDbContextFixture _dbFixture;
        private readonly IMapper _mapper;

        public ContactPersonServiceTests(AlertingDbContextFixture dbFixture, AutoMapperFixture mapperFixture)
        {
            _dbFixture = dbFixture;
            _mapper = mapperFixture.GetMapper();
        }

        [Theory]
        [InlineData("meysam", "", 5)]
        [InlineData("", "abasi", 5)]
        [InlineData("", "", 5)]
        [InlineData("something", "", 0)]
        public void Get_contact_persons_count(string fname, string lname, int expectedCount)
        {
            // arrange
            using (var context = _dbFixture.GetDbContext())
            {
                context.ClearData();

                var repository = new ContactPersonRepository(context);
                var service = new ContactPersonService(repository, _mapper);
                for (int i = 0; i < 5; i++)
                {
                    var contactPerson = CreateContactPerson("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
                    service.CreateContactPerson(contactPerson);
                }
            }

            int actualCount;
            using (var context = _dbFixture.GetDbContext())
            {
                var filter = new ContactPersonsFilter { FirstName = fname, LastName = lname };
                var repository = new ContactPersonRepository(context);
                var sut = new ContactPersonService(repository, _mapper);

                // act
                actualCount = sut.GetContactPersonsCount(filter);
            }

            // assert
            actualCount.Should().Be(expectedCount);
        }

        [Theory]
        [InlineData("meysam", "", 3, 2, 2)]
        [InlineData("", "abasi", 2, 3, 3)]
        [InlineData("", "", 3, 3, 2)]
        [InlineData("something", "", 0, 5, 0)]
        public void Get_contact_persons(string fname, string lname, int skip, int limit, int expectedCount)
        {
            // arrange
            using (var context = _dbFixture.GetDbContext())
            {
                context.ClearData();

                var repository = new ContactPersonRepository(context);
                var service = new ContactPersonService(repository, _mapper);
                for (int i = 0; i < 5; i++)
                {
                    var contactPerson = CreateContactPerson("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
                    service.CreateContactPerson(contactPerson);
                }
            }

            IEnumerable<ContactPersonListItem> contactPersons;
            using (var context = _dbFixture.GetDbContext())
            {
                var filter = new ContactPersonsPagableFilter { FirstName = fname, LastName = lname, Skip = skip, Limit = limit };
                var repository = new ContactPersonRepository(context);
                var sut = new ContactPersonService(repository, _mapper);

                // act
                contactPersons = sut.GetContactPersons(filter).ToArray();
            }

            contactPersons.Count().Should().Be(expectedCount);
        }

        [Fact]
        public void Create_contact_person()
        {
            // arrange
            using (var context = _dbFixture.GetDbContext())
            {
                context.ClearData();
            }

            var contactPerson = CreateContactPerson("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            using (var context = _dbFixture.GetDbContext())
            {
                var repository = new ContactPersonRepository(context);
                var sut = new ContactPersonService(repository, _mapper);

                // act
                contactPerson = sut.CreateContactPerson(contactPerson);
            }

            // assert
            contactPerson.Id.Should().BeGreaterThan(0);
            using (var context = _dbFixture.GetDbContext())
            {
                var repository = new ContactPersonRepository(context);
                var contactPersonFromDb = repository.GetContactPersonById(contactPerson.Id);
                contactPersonFromDb.Should().Equals(contactPerson);
            }
        }

        [Fact]
        public void Update_contact_person()
        {
            // arrange
            var updatedContactPerson = CreateContactPerson("meysam2", "abasi2", "09126143807", "abasi.maisam@gmail.com");
            var contactPerson = CreateContactPerson("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            using (var context = _dbFixture.GetDbContext())
            {
                context.ClearData();
                var repository = new ContactPersonRepository(context);
                var service = new ContactPersonService(repository, _mapper);
                contactPerson = service.CreateContactPerson(contactPerson);
            }

            using (var context = _dbFixture.GetDbContext())
            {
                var repository = new ContactPersonRepository(context);
                var sut = new ContactPersonService(repository, _mapper);

                // act
                updatedContactPerson = sut.UpdateContactPerson(contactPerson.Id, updatedContactPerson);
            }

            // assert
            using (var context = _dbFixture.GetDbContext())
            {
                var repository = new ContactPersonRepository(context);
                var contactPersonFromDb = repository.GetContactPersonById(updatedContactPerson.Id);
                contactPersonFromDb.ShouldBeEquivalentTo(updatedContactPerson);
            }
        }

        [Fact]
        public void Delete_contact_person()
        {
            // arrange
            var contactPerson = CreateContactPerson("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            using (var context = _dbFixture.GetDbContext())
            {
                context.ClearData();
                var repository = new ContactPersonRepository(context);
                var service = new ContactPersonService(repository, _mapper);
                contactPerson = service.CreateContactPerson(contactPerson);
            }

            using (var context = _dbFixture.GetDbContext())
            {
                var repository = new ContactPersonRepository(context);
                var sut = new ContactPersonService(repository, _mapper);

                // act
                sut.DeleteContactPerson(contactPerson.Id);
            }

            // assert
            using (var context = _dbFixture.GetDbContext())
            {
                var repository = new ContactPersonRepository(context);
                var contactPersonFromDb = repository.GetContactPersonById(contactPerson.Id);
                contactPersonFromDb.Should().BeNull();
            }
        }

        private ContactPerson CreateContactPerson(string fname, string lname, string phone, string email)
        {
            return new ContactPerson
            {
                FirstName = fname,
                LastName = lname,
                PhoneNumber = phone,
                Email = email
            };
        }
    }
}
