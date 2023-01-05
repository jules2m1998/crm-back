using CRM.core.DataAccess;
using CRM.core.Services;

namespace CRM.api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectAllDependencies(this IServiceCollection services)
        {
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IDataAccess, DataAccess>();

            return services;
        }
    }
}
