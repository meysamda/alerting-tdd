using Alerting.Application.ContactPersons;
using Alerting.Infrastructure.Data.DbContexts;
using Alerting.Infrastructure.Data.Repositories;
using Alerting.Presentation.ErrorHandling;
using Alerting.Presentation.Init;
using Alerting.Presentation.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper();

            var connectionString = Configuration.GetConnectionString("Default");
            var migrationsAssembly = typeof(AlertingDbContext).Assembly.FullName;

            services.AddDbContext<AlertingDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly))
            );

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://idp.tech1a.co";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                });

            services.AddScoped<ContactPersonService>();
            services.AddScoped<ContactPersonRepository>();

            services.AddCustomizedSwagger(Configuration);
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
        }
    }
}
