using Alerting.Tests.AcceptanceTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alerting.Tests.AcceptanceTests.ContactPersons
{
    public class ContactPersonApiClientFixture : AlertingApiClientFixture
    {
        private const string CONTACT_PERSONS_RELATIVE_URL = "api/contact-persons";

        public ValueTask<int> GetContactPersonsCount(ContactPersonsFilter filter, bool authorized = true)
        {
            var parameters = new List<Tuple<string, string>>();
            parameters.Add(new Tuple<string, string>("FirstName", filter.FirstName));
            parameters.Add(new Tuple<string, string>("LastName", filter.LastName));
            var queryString = string.Join("&", parameters
                .Where(o => !string.IsNullOrEmpty(o.Item2))
                .Select(o => $"{o.Item1}={o.Item2}")
                .ToArray());

            var relativeUrl = CONTACT_PERSONS_RELATIVE_URL + $"/count?{queryString}";

            return authorized ?
                _authorizedClient.GetContentAsync<int>(relativeUrl) :
                _client.GetContentAsync<int>(relativeUrl);
        }

        public ValueTask<IEnumerable<ContactPersonListItem>> GetContactPersons(ContactPersonsPagableFilter filter, bool authorized = true)
        {
            var parameters = new List<Tuple<string, string>>();
            parameters.Add(new Tuple<string, string>("FirstName", filter.FirstName));
            parameters.Add(new Tuple<string, string>("LastName", filter.LastName));
            parameters.Add(new Tuple<string, string>("Skip", filter.Skip.ToString()));
            parameters.Add(new Tuple<string, string>("Limit", filter.Limit.ToString()));
            parameters.Add(new Tuple<string, string>("Sort", filter.Sort));
            var queryString = string.Join("&", parameters
                .Where(o => !string.IsNullOrEmpty(o.Item2))
                .Select(o => $"{o.Item1}={o.Item2}")
                .ToArray());

            var relativeUrl = CONTACT_PERSONS_RELATIVE_URL + $"?{queryString}";

            return authorized ?
                _authorizedClient.GetContentAsync<IEnumerable<ContactPersonListItem>>(relativeUrl) :
                _client.GetContentAsync<IEnumerable<ContactPersonListItem>>(relativeUrl);
        }

        public ValueTask<int> CreateContactPerson(CreateContactPersonCommand contactPerson, bool authorized = true)
        {
            var relativeUrl = CONTACT_PERSONS_RELATIVE_URL;

            return authorized ?
                _authorizedClient.PostContentAsync<CreateContactPersonCommand, int>(relativeUrl, contactPerson) :
                _client.PostContentAsync<CreateContactPersonCommand, int>(relativeUrl, contactPerson);
        }

        public ValueTask<CreateContactPersonCommand> UpdateContactPerson(int id, CreateContactPersonCommand updatedContactPerson, bool authorized = true)
        {
            var relativeUrl = CONTACT_PERSONS_RELATIVE_URL + $"/{id}";

            return authorized ?
                _authorizedClient.PutContentAsync<CreateContactPersonCommand>(relativeUrl, updatedContactPerson) :
                _client.PutContentAsync<CreateContactPersonCommand>(relativeUrl, updatedContactPerson);
        }

        public ValueTask DeleteContactPerson(int id, bool authorized = true)
        {
            var relativeUrl = CONTACT_PERSONS_RELATIVE_URL + $"/{id}";

            return authorized ?
                _authorizedClient.DeleteContentAsync(relativeUrl) :
                _client.DeleteContentAsync(relativeUrl);
        }
    }
}
