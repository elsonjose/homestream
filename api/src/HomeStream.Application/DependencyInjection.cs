using HomeStream.Application.Implementations.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HomeStream.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        RegisterApplicationServices(services);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }

    private static IServiceCollection RegisterApplicationServices(IServiceCollection services)
    {
        return services.AddTransient<FileScannerService>();
    }
}
