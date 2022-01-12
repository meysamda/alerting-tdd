using System;
using System.Threading.Tasks;
using Alerting.Tests.AcceptanceTests.Helpers;
using Alerting.Tests.AcceptanceTests.Models;
using FluentAssertions;
using Xunit;

namespace Alerting.Tests.AcceptanceTests
{
    public class ContactPersonsControllerTests : IClassFixture<AlertingApiClientFixture>
    {
        private readonly AlertingApiClientFixture _alertingApiClientFixture;

        public ContactPersonsControllerTests(AlertingApiClientFixture alertingApiClientFixture)
        {
            _alertingApiClientFixture = alertingApiClientFixture;
        }

        [Fact]
        public async Task Authorized_user_posts_valid_contact_person()
        {
            var contactPerson = CreateValidContactPerson();

            var response = await _alertingApiClientFixture.PostContactPersonAsync(contactPerson, false);

            response.Should().BeGreaterThan(0);
        }

        // [Fact]
        // public async Task Post_invalid_contact_person()
        // {
        //     var contactPerson = CreateInvalidContactPerson();

        //     var response = await _alertingApiClient.PostContactPersonAsync(contactPerson);

        //     response.Should().BeOfType(typeof(BadRequestErrorResponse));
        // }

        private ContactPerson CreateValidContactPerson()
        {
            return new ContactPerson {
                FirstName = "meysam",
                LastName = "abasi",
                PhoneNumber = "09126143808",
                Email = "abasi.maisam@gmail.com"
            };
        }

        // private ContactPerson CreateInvalidContactPerson()
        // {
        //     return new ContactPerson {
        //         FirstName = "meysam",
        //         LastName = "abasi"
        //     };
        // }
    }
}
