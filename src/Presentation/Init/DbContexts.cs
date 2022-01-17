using Alerting.Infrastructure.Data.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Alerting.Presentation.Init
{
    public static class DbContexts
    {
        public static void AddAlertingDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            var migrationsAssembly = typeof(AlertingDbContext).Assembly.FullName;

            services.AddDbContext<AlertingDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly))
            );
        }

        public static void MigrateAlertingDbContext(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AlertingDbContext>();
            dbContext.Database.Migrate();
        }
    }
}