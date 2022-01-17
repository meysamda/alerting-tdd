using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using RESTFulSense.Exceptions;
using Xunit;

namespace Alerting.Tests.AcceptanceTests.ContactPersons
{
    public class ContactPersonsControllerTests : IClassFixture<ContactPersonApiClientFixture>
    {
        private readonly ContactPersonApiClientFixture _clientFixture;

        public ContactPersonsControllerTests(ContactPersonApiClientFixture clientFixture)
        {
            _clientFixture = clientFixture;
        }

        #region Get contact persons count
        [Theory]
        [InlineData("mey", "", 1)]
        [InlineData("", "abasi", 2)]
        public async Task Authorized_user_gets_contact_persons_counts_by_filter(string fname, string lname, int expectedCount)
        {
            // arrange
            await ClearContactPersons();
            var cmdArray = new CreateContactPersonCommand[]
            {
                new CreateContactPersonCommand("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com"),
                new CreateContactPersonCommand("reza", "zare", "09362966111", "reza.zare@gmail.com"),
                new CreateContactPersonCommand("hamid", "derakhshsan", "09123456789", "hamid.derakhshan@gmail.com"),
                new CreateContactPersonCommand("ali", "bahri", "09012543680", "ali.bahri@gmail.com"),
                new CreateContactPersonCommand("yasaman", "abasi", "09126153838", "yasaman.bahri@gmail.com"),
                new CreateContactPersonCommand("zahra", "jenabi", "09189072622", "zahra.jenabi@gmail.com"),
                new CreateContactPersonCommand("alireza", "rezaie", "09112544478", "alireza.rezaie@gmail.com"),
                new CreateContactPersonCommand("bahman", "samadi", "09188112544", "bahman.samadi@gmail.com")
            };
         
            foreach (var cmd in cmdArray)
                await _clientFixture.CreateContactPerson(cmd);

            var filter = new ContactPersonsFilter { FirstName = fname, LastName = lname };

            // act
            var actualCount = await _clientFixture.GetContactPersonsCount(filter);

            // assert
            actualCount.Should().Be(expectedCount);
        }

        [Fact]
        public async Task Unauthorized_user_gets_contact_persons_counts_by_filter()
        {
            // arrange
            var filter = new ContactPersonsFilter();
            
            // assert
            await Assert.ThrowsAsync<HttpResponseUnauthorizedException>(() =>
            {
                // act
                return _clientFixture.GetContactPersonsCount(filter, false).AsTask();
            });
        }
        #endregion

        #region Get contact persons by filter
        [Theory]
        [InlineData("meysam", "", 3, 2, 0)]
        [InlineData("meysam", "", 0, 2, 1)]
        [InlineData("", "abasi", 2, 3, 0)]
        [InlineData("", "abasi", 0, 1, 1)]
        [InlineData("", "", 3, 3, 3)]
        [InlineData("something", "", 0, 5, 0)]
        public async Task Authorized_user_gets_contact_persons_by_filter(string fname, string lname, int skip, int limit, int expectedCount)
        {
            // arrange
            await ClearContactPersons();
            var cmdArray = new CreateContactPersonCommand[]
            {
                new CreateContactPersonCommand("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com"),
                new CreateContactPersonCommand("reza", "zare", "09362966111", "reza.zare@gmail.com"),
                new CreateContactPersonCommand("hamid", "derakhshsan", "09123456789", "hamid.derakhshan@gmail.com"),
                new CreateContactPersonCommand("ali", "bahri", "09012543680", "ali.bahri@gmail.com"),
                new CreateContactPersonCommand("yasaman", "abasi", "09126153838", "yasaman.bahri@gmail.com"),
                new CreateContactPersonCommand("zahra", "jenabi", "09189072622", "zahra.jenabi@gmail.com"),
                new CreateContactPersonCommand("alireza", "rezaie", "09112544478", "alireza.rezaie@gmail.com"),
                new CreateContactPersonCommand("bahman", "samadi", "09188112544", "bahman.samadi@gmail.com")
            };

            foreach (var cmd in cmdArray)
                await _clientFixture.CreateContactPerson(cmd);

            var filter = new ContactPersonsPagableFilter { FirstName = fname, LastName = lname, Skip = skip, Limit = limit };

            // act
            var contactPersons = await _clientFixture.GetContactPersons(filter);

            // assert
            contactPersons.Count().Should().Be(expectedCount);
        }

        [Fact]
        public async Task Unauthorized_user_gets_contact_persons_by_filter()
        {
            // arrange
            var filter = new ContactPersonsPagableFilter();

            // assert
            await Assert.ThrowsAsync<HttpResponseUnauthorizedException>(() =>
            {
                // act
                return _clientFixture.GetContactPersons(filter, false).AsTask();
            });
        }
        #endregion

        #region Create contact person
        [Fact]
        public async Task Authorized_user_creates_valid_contact_person()
        {
            var command = new CreateContactPersonCommand("meysame", "abasi", "09126143808", "abasi.maisam@gmail.com");

            var response = await _clientFixture.CreateContactPerson(command);

            response.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("meysam", "abasi", "09123", "asdf.com")]
        [InlineData("meysam", "abasi", "09126143808", "asdf.com")]
        public async Task Authorized_user_creates_invalid_contact_person(string fname, string lname, string phone, string email)
        {
            var command = new CreateContactPersonCommand(fname, lname, phone, email);

            // assert
            await Assert.ThrowsAsync<HttpResponseBadRequestException>(() =>
            {
                // act
                return _clientFixture.CreateContactPerson(command).AsTask();
            });
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("meysame", "abasi", "09126143808", "abasi.maisam@gmail.com")]
        public async Task Unauthorized_user_creates_contact_person(string fname, string lname, string phone, string email)
        {
            var command = new CreateContactPersonCommand(fname, lname, phone, email);

            // assert
            await Assert.ThrowsAsync<HttpResponseUnauthorizedException>(() =>
            {
                // act
                return _clientFixture.CreateContactPerson(command, false).AsTask();
            });
        }
        #endregion

        #region Update contact person
        [Fact]
        public async Task Authorized_user_updates_an_existing_contact_person_to_valid_contact_person()
        {
            var createCommand = new CreateContactPersonCommand("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            var id = await _clientFixture.CreateContactPerson(createCommand);
            var updateCommand = new UpdateContactPersonCommand("meysam2", "abasi2", "09126143807", "abasi2.maisam@gmail.com");

            await _clientFixture.UpdateContactPerson(id, updateCommand);
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("meysam", "abasi", "09123", "asdf.com")]
        [InlineData("meysam", "abasi", "09126143808", "asdf.com")]
        public async Task Authorized_user_updates_an_existing_contact_person_to_invalid_contact_person(string fname, string lname, string phone, string email)
        {
            var createCommand = new CreateContactPersonCommand("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            var id = await _clientFixture.CreateContactPerson(createCommand);
            var updateCommand = new UpdateContactPersonCommand(fname, lname, phone, email);

            // assert
            await Assert.ThrowsAsync<HttpResponseBadRequestException>(() =>
            {
                // act
                return _clientFixture.UpdateContactPerson(id, updateCommand).AsTask();
            });
        }

        [Fact]
        public async Task Authorized_user_updates_a_non_existing_contact_person_to_valid_contact_person()
        {
            var id = -1;
            var command = new UpdateContactPersonCommand("meysam2", "abasi2", "09126143807", "abasi2.maisam@gmail.com");

            // assert
            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
            {
                // act
                return _clientFixture.UpdateContactPerson(id, command).AsTask();
            });
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("meysame", "abasi", "09126143808", "abasi.maisam@gmail.com")]
        public async Task Unauthorized_user_updates_contact_person(string fname, string lname, string phone, string email)
        {
            var createCommand = new CreateContactPersonCommand("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            var id = await _clientFixture.CreateContactPerson(createCommand);
            var updateCommand = new UpdateContactPersonCommand(fname, lname, phone, email);

            // assert
            await Assert.ThrowsAsync<HttpResponseUnauthorizedException>(() =>
            {
                // act
                return _clientFixture.UpdateContactPerson(id, updateCommand, false).AsTask();
            });
        }
        #endregion

        #region Delete contact person
        [Fact]
        public async Task Authorized_user_deletes_an_existing_contact_person()
        {
            var command = new CreateContactPersonCommand("meysame", "abasi", "09126143808", "abasi.maisam@gmail.com");
            var id = await _clientFixture.CreateContactPerson(command);

            await _clientFixture.DeleteContactPerson(id);

            // assert
        }

        [Fact]
        public async Task Authorized_user_deletes_a_non_existing_contact_person()
        {
            var id = -1;

            // assert
            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
            {
                // act
                return _clientFixture.DeleteContactPerson(id).AsTask();
            });
        }

        [Fact]
        public async Task Unauthorized_user_deletes_a_contact_person()
        {
            var id = -1;

            // assert
            await Assert.ThrowsAsync<HttpResponseUnauthorizedException>(() =>
            {
                // act
                return _clientFixture.DeleteContactPerson(id, false).AsTask();
            });
        }
        #endregion

        private async Task ClearContactPersons()
        {
            const int PAGE_SIZE = 100;
            var filter = new ContactPersonsFilter();
            var count = await _clientFixture.GetContactPersonsCount(filter);
            var pagesCount = ((count - 1) / PAGE_SIZE) + 1;
            var existingListItems = new List<ContactPersonListItem>();
            for (int page = 0; page < pagesCount; page++)
            {
                var pagableFilter = new ContactPersonsPagableFilter
                {
                    Skip = page * PAGE_SIZE,
                    Limit = PAGE_SIZE
                };
                var pageListItems = await _clientFixture.GetContactPersons(pagableFilter);
                existingListItems.AddRange(pageListItems);
            }
            foreach (var item in existingListItems)
            {
                await _clientFixture.DeleteContactPerson(item.Id);
            }
        }
    }
}
