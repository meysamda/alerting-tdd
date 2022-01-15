using Alerting.Application.ContactPersons;
using Alerting.Infrastructure.Data.DbContexts.Entities;
using Alerting.Infrastructure.Data.Repositories;
using Alerting.Tests.IntegrationTests.Helpers;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Alerting.Tests.IntegrationTests
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
