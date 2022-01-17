using Alerting.Application.ContactPersons;
using Alerting.Infrastructure.Data.Repositories.ContactPersons;
using Alerting.Presentation.ErrorHandling;
using Alerting.Presentation.Init;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Alerting.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper();

            services.AddAlertingDbContext(Configuration);

            services.AddCustomizedAuthentication(Configuration);

            services.AddCustomizedSwagger(Configuration);

            services.AddScoped<ContactPersonService>();
            services.AddScoped<ContactPersonRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomizedExceptionHandler();
            app.UseSwaggerAndSwaggerUI(Configuration);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MigrateAlertingDbContext();
        }
    }
}
