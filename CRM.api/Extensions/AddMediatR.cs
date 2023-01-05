using CRM.core;
using MediatR;

namespace CRM.api.Extensions
{
    public static class AddMediatR
    {
        public static IServiceCollection AddMediatRConf(this IServiceCollection services)
        {
           services.AddMediatR(typeof(MediatorEntry).Assembly);

            return services;
        }
    }
}
