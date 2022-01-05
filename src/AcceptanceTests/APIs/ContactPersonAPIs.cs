using System;
using System.Threading.Tasks;
using Alerting.AcceptanceTests.Helpers;
using Alerting.AcceptanceTests.Models;
using FluentAssertions;
using Xunit;

namespace Alerting.AcceptanceTests.APIs
{
    public class ContactPersonAPIs
    {
        private readonly AlertingApiClient _alertingApiClient;

        public ContactPersonAPIs(AlertingApiClient alertingApiClient)
        {
            _alertingApiClient = alertingApiClient;
        }

        [Fact]
        public async Task Create_contact_person()
        {
            var contactPerson = CreateContactPerson();

            var response = await _alertingApiClient.PostContactPersonAsync(contactPerson);

            response.Should().BeGreaterThan(0);
        }

        private ContactPerson CreateContactPerson()
        {
            return new ContactPerson {
                FirstName = "meysam",
                LastName = "abasi",
                PhoneNumber = "09126143808",
                Email = "abasi.maisam@gmail.com"
            };
        }
    }
}
