using System;
using Microsoft.Extensions.Configuration;

namespace Alerting.Tests.IntegrationTests.Common
{
    public static class TestConfigurationBuilder
    {
        public static IConfiguration Build()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", false);
            builder.AddJsonFile($"appsettings.{environmentName}.json", true);

            return builder.Build();
        }
    }
}