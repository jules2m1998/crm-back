using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CRM.Core.Business;

public static class BusinessServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection @this)
    {
        @this.AddAutoMapper(Assembly.GetExecutingAssembly());
        @this.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return @this;
    }
}
