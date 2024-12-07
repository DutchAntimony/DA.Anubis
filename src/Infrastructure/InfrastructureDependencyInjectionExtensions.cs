using DA.DDD.InfraLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace DA.Anubis.Infrastructure;

public static class InfrastructureDependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSystemDateTimeProvider();
        services.AddSystemUserProvider();
        return services;
    }
}