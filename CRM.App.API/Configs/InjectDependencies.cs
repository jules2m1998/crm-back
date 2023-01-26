using CRM.Core.Business.Authentication;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Repositories;
using CRM.Infra.Data.Helpers;
using CRM.Infra.Data.Repositories;
using CRM.Infra.Services;

namespace CRM.App.API.Configs
{
    public static class InjectDependencies
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileHelper, FileHelper>();

            return services;
        }
    }
}
