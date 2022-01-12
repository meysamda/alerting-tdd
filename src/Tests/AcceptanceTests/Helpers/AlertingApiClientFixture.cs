using Alerting.Presentation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using RESTFulSense.Clients;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Alerting.Tests.AcceptanceTests.Helpers
{
    public partial class AlertingApiClientFixture
    {
        private readonly IRESTFulApiFactoryClient _apiFactoryClient;
        private readonly IRESTFulApiFactoryClient _apiFactoryAutorizedClient;

        public AlertingApiClientFixture()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", 
                        options =>
                        {
                            Console.WriteLine("hehe");
                        });
                });
            });

            var client = webApplicationFactory.CreateClient();
            var authorizedClient = webApplicationFactory.CreateClient();
            authorizedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

            _apiFactoryClient = new RESTFulApiFactoryClient(client);
            _apiFactoryAutorizedClient = new RESTFulApiFactoryClient(authorizedClient);
        }
    }
}
