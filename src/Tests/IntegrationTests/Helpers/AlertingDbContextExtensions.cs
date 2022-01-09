using System;
using Alerting.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alerting.Tests.IntegrationTests.Helpers
{
    public static class AlertingDbContextExtensions
    {
        public static void ClearData(this AlertingDbContext dbContext)
        {
            var commandText = "DELETE FROM dbo.[ContactPersons];";
            dbContext.Database.ExecuteSqlRaw(commandText);
        }
    }
}