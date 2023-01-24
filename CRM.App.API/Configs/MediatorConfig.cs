using CRM.Core.Business;
using MediatR;
using System.Reflection;

namespace CRM.App.API.Configs
{
    public static class MediatorConfig
    {
        public static IServiceCollection AddMediaRConfig(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.AsScoped();
            }, Assembly.GetAssembly(typeof(MediatREntryPoint)));
            return services;
        }
    }
}
