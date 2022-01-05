using Microsoft.AspNetCore.Mvc.Testing;
using RESTFulSense.Clients;

namespace Alerting.AcceptanceTests.Helpers
{
    public partial class AlertingApiClient
    {
        private readonly IRESTFulApiFactoryClient _apiFactoryClient;

        public AlertingApiClient()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            var baseClient = webApplicationFactory.CreateClient();
            _apiFactoryClient = new RESTFulApiFactoryClient(baseClient);
        }
    }
}
