using Alerting.Tests.AcceptanceTests.Common;
using System.Threading.Tasks;

namespace Alerting.Tests.AcceptanceTests.ContactPersons
{
    public class ContactPersonApiClientFixture : AlertingApiClientFixture
    {
        private const string CONTACT_PERSONS_RELATIVE_URL = "api/contact-persons";

        public ValueTask<int> CreateContactPersonAsync(CreateContactPersonCommand contactPerson, bool authorized = true)
        {
            var relativeUrl = CONTACT_PERSONS_RELATIVE_URL;

            return authorized ?
                _authorizedClient.PostContentAsync<CreateContactPersonCommand, int>(relativeUrl, contactPerson) :
                _client.PostContentAsync<CreateContactPersonCommand, int>(relativeUrl, contactPerson);
        }

        public ValueTask<CreateContactPersonCommand> UpdateContactPersonAsync(int id, CreateContactPersonCommand updatedContactPerson, bool authorized = true)
        {
            var relativeUrl = CONTACT_PERSONS_RELATIVE_URL + $"/{id}";

            return authorized ?
                _authorizedClient.PutContentAsync<CreateContactPersonCommand>(relativeUrl, updatedContactPerson) :
                _client.PutContentAsync<CreateContactPersonCommand>(relativeUrl, updatedContactPerson);
        }

        public ValueTask DeleteContactPersonAsync(int id, bool authorized = true)
        {
            var relativeUrl = CONTACT_PERSONS_RELATIVE_URL + $"/{id}";

            return authorized ?
                _authorizedClient.DeleteContentAsync(relativeUrl) :
                _client.DeleteContentAsync(relativeUrl);
        }
    }
}
