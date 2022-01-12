using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Alerting.Presentation.Init
{
    public static class AutoMapper
    {
        public static void AddAutoMapper(this IServiceCollection services)
        {
            var assemblyNames = new string[] { "Presentation" };
            var assemblies = assemblyNames.Select(o => Assembly.Load(o)).ToArray();

            var profiles = assemblies
                .SelectMany(o => o.GetExportedTypes())
                .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()))
                .Where(t => !t.GetTypeInfo().IsAbstract);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                foreach (var profile in profiles)
                {
                    mc.AddProfile(profile);
                }
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
