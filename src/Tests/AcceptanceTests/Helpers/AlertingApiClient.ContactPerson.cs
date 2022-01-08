using System.Threading.Tasks;
using Alerting.Tests.AcceptanceTests.Models;

namespace Alerting.Tests.AcceptanceTests.Helpers
{
    public partial class AlertingApiClient
    {
        private const string CONTACT_PERSONS_RELATIVE_URL = "api/contact-persons";

        public async ValueTask<int> PostContactPersonAsync(ContactPerson contactPerson) => 
            await this._apiFactoryClient.PostContentAsync<ContactPerson, int>(CONTACT_PERSONS_RELATIVE_URL, contactPerson);
    }
}
