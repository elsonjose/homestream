using HomeStream.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HomeStream.Application.Abstractions.Persistence;

public interface IHomeStreamDbContext
{
    public DbSet<JobDetail> JobDetails { get; set; }

    public Task<int> PersistChangesAsync(CancellationToken cancellationToken);

    public EntityEntry Modify<IEntity>(IEntity entity);
}
