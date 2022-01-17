using System;
using Alerting.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alerting.Tests.IntegrationTests.Common
{
    public class AlertingDbContextFixture
    {
        private readonly DbContextOptions<AlertingDbContext> _options;

        public AlertingDbContextFixture()
        {
            var configuration = TestConfigurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("default");
            _options = new DbContextOptionsBuilder<AlertingDbContext>().UseSqlServer(connectionString, 
                sql => sql.MigrationsAssembly(typeof(AlertingDbContext).Assembly.FullName))
                .Options;
        }

        public AlertingDbContext GetDbContext()
        {
            var dbContext = new AlertingDbContext(_options);
            return dbContext;
        }

        public void MigrateDbContext()
        {
            var dbContext = new AlertingDbContext(_options);
            dbContext.Database.Migrate();
        }
    }
}