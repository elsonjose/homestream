using static HomeStream.Domain.Core.HomeStreamEnums;

namespace HomeStream.Domain.Entities;

public class JobDetail
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Payload { get; set; }

    public double Progress { get; set; }

    public string ExceptionMessage { get; set; }

    public string StrackTrace { get; set; }

    public long CreatedOn { get; set; }

    public long StartedOn { get; set; }

    public long CompletedOn { get; set; }

    public JobStatus Status { get; set; }

}
