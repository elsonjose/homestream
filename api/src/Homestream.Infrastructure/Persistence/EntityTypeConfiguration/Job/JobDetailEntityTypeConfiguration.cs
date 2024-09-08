using HomeStream.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static HomeStream.Domain.Core.HomeStreamEnums;

namespace HomeStream.Infrastructure.Persistence.EntityTypeConfiguration;

public class JobDetailEntityTypeConfiguration : IEntityTypeConfiguration<JobDetail>
{
    public void Configure(EntityTypeBuilder<JobDetail> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Status).HasDefaultValue(JobStatus.Queued);
        builder.Property(x => x.Progress).HasDefaultValue(0);
        builder.Property(x => x.ExceptionMessage).HasDefaultValue(string.Empty);
        builder.Property(x => x.StackTrace).HasDefaultValue(string.Empty);
    }
}
