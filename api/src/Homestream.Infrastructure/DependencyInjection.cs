using HomeStream.Application.Abstractions.Persistence;
using HomeStream.Application.Common;
using HomeStream.Domain.Abstractions.Services;
using HomeStream.Infrastructure.HostServices;
using HomeStream.Infrastructure.Implementations;
using HomeStream.Infrastructure.Persistence.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace HomeStream.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        AddDatabase(services, configuration);
        AddRedisConfig(services, configuration);
        services.AddHostedService<HomestreamHostedService>();
        services.AddSingleton<IJobExecutionService, JobExecutionService>();
        services.AddSingleton<ICacheService, RedisCacheService>();
        return services;
    }

    private static void AddRedisConfig(IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis")
            .ThrowExceptionIfNullOrEmptyWithError("Redis connection string not found in configuration");
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse(connectionString);
            return ConnectionMultiplexer.Connect(configuration);
        });
    }

    private static void AddDatabase(IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString")
            .ThrowExceptionIfNullOrEmptyWithError("DefaultConnectionString not found in configuration");
        services.AddDbContext<IHomeStreamDbContext, HomeStreamDbContext>(
            options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention()
        );
    }
}
