using HomeStream.Application.Abstractions.Persistence;
using HomeStream.Infrastructure.Persistence.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeStream.Infrastructure;

public static class HomestreamServices
{
    public static IApplicationBuilder UseInfrastureServices(this WebApplication app)
    {
        MigrateDatabase(app);
        return app;
    }

    private static void MigrateDatabase(WebApplication app)
    {
        var dbContext = (HomeStreamDbContext)app.Services.CreateScope().ServiceProvider.GetRequiredService<IHomeStreamDbContext>();
        dbContext.Database.Migrate();
    }
}
