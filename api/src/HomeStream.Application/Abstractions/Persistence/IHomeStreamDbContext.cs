using HomeStream.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeStream.Application.Abstractions.Persistence;

public interface IHomeStreamDbContext
{
    public DbSet<JobDetail> JobDetails { get; set; }
}
