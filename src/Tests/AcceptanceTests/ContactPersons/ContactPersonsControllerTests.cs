using System;
using System.Threading.Tasks;
using Alerting.Tests.AcceptanceTests.Common;
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

        #region Create contact person
        [Fact]
        public async Task Authorized_user_posts_valid_contact_person()
        {
            var command = new CreateContactPersonCommand("meysame", "abasi", "09126143808", "abasi.maisam@gmail.com");

            var response = await _clientFixture.CreateContactPersonAsync(command);

            response.Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("meysam", "abasi", "09123", "asdf.com")]
        [InlineData("meysam", "abasi", "09126143808", "asdf.com")]
        public async Task Authorized_user_posts_invalid_contact_person(string fname, string lname, string phone, string email)
        {
            var command = new CreateContactPersonCommand(fname, lname, phone, email);

            // assert
            await Assert.ThrowsAsync<HttpResponseBadRequestException>(() =>
            {
                // act
                return _clientFixture.CreateContactPersonAsync(command).AsTask();
            });
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("meysame", "abasi", "09126143808", "abasi.maisam@gmail.com")]
        public async Task Unauthorized_user_posts_contact_person(string fname, string lname, string phone, string email)
        {
            var command = new CreateContactPersonCommand(fname, lname, phone, email);

            // assert
            await Assert.ThrowsAsync<HttpResponseUnauthorizedException>(() =>
            {
                // act
                return _clientFixture.CreateContactPersonAsync(command, false).AsTask();
            });
        }
        #endregion

        #region Update contact person
        [Fact]
        public async Task Authorized_user_updates_an_existing_contact_person_to_valid_contact_person()
        {
            var createCommand = new CreateContactPersonCommand("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            var id = await _clientFixture.CreateContactPersonAsync(createCommand);
            var updateCommand = new UpdateContactPersonCommand("meysam2", "abasi2", "09126143807", "abasi2.maisam@gmail.com");

            await _clientFixture.UpdateContactPersonAsync(id, updateCommand);
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("meysam", "abasi", "09123", "asdf.com")]
        [InlineData("meysam", "abasi", "09126143808", "asdf.com")]
        public async Task Authorized_user_updates_an_existing_contact_person_to_invalid_contact_person(string fname, string lname, string phone, string email)
        {
            var createCommand = new CreateContactPersonCommand("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            var id = await _clientFixture.CreateContactPersonAsync(createCommand);
            var updateCommand = new UpdateContactPersonCommand(fname, lname, phone, email);

            // assert
            await Assert.ThrowsAsync<HttpResponseBadRequestException>(() =>
            {
                // act
                return _clientFixture.UpdateContactPersonAsync(id, updateCommand).AsTask();
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
                return _clientFixture.UpdateContactPersonAsync(id, command).AsTask();
            });
        }

        [Theory]
        [InlineData("", "", "", "")]
        [InlineData("meysame", "abasi", "09126143808", "abasi.maisam@gmail.com")]
        public async Task Unauthorized_user_updates_contact_person(string fname, string lname, string phone, string email)
        {
            var createCommand = new CreateContactPersonCommand("meysam", "abasi", "09126143808", "abasi.maisam@gmail.com");
            var id = await _clientFixture.CreateContactPersonAsync(createCommand);
            var updateCommand = new UpdateContactPersonCommand(fname, lname, phone, email);

            // assert
            await Assert.ThrowsAsync<HttpResponseUnauthorizedException>(() =>
            {
                // act
                return _clientFixture.UpdateContactPersonAsync(id, updateCommand, false).AsTask();
            });
        }
        #endregion

        #region Delete contact person
        [Fact]
        public async Task Authorized_user_deletes_an_existing_contact_person()
        {
            var command = new CreateContactPersonCommand("meysame", "abasi", "09126143808", "abasi.maisam@gmail.com");
            var id = await _clientFixture.CreateContactPersonAsync(command);

            await _clientFixture.DeleteContactPersonAsync(id);

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
                return _clientFixture.DeleteContactPersonAsync(id).AsTask();
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
                return _clientFixture.DeleteContactPersonAsync(id, false).AsTask();
            });
        }
        #endregion
    }
}
