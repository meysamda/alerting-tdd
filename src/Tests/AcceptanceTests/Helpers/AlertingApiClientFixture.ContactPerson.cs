using System.Threading.Tasks;
using Alerting.Tests.AcceptanceTests.Models;

namespace Alerting.Tests.AcceptanceTests.Helpers
{
    public partial class AlertingApiClientFixture
    {
        private const string CONTACT_PERSONS_RELATIVE_URL = "api/contact-persons";

        public async ValueTask<int> PostContactPersonAsync(ContactPerson contactPerson, bool authorizedClient = true)
        {
            return authorizedClient ?
                await _apiFactoryClient.PostContentAsync<ContactPerson, int>(CONTACT_PERSONS_RELATIVE_URL, contactPerson) :
                await _apiFactoryAutorizedClient.PostContentAsync<ContactPerson, int>(CONTACT_PERSONS_RELATIVE_URL, contactPerson);
        }
    }
}
