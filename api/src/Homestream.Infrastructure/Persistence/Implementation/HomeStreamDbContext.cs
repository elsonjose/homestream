using HomeStream.Application.Abstractions.Persistence;
using HomeStream.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HomeStream.Infrastructure.Persistence.Implementation;

public class HomeStreamDbContext(DbContextOptions<HomeStreamDbContext> options) : DbContext(options), IHomeStreamDbContext
{
    #region DB sets

    public DbSet<JobDetail> JobDetails { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
