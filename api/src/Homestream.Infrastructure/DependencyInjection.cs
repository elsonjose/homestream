using HomeStream.Application.Abstractions.Persistence;
using HomeStream.Infrastructure.Persistence.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HomeStream.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        AddDatabase(services, configuration);

        return services;
    }

    private static void AddDatabase(IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<IHomeStreamDbContext, HomeStreamDbContext>(
            options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
        );
    }
}
