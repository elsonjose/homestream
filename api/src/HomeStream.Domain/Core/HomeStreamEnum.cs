namespace HomeStream.Domain.Core;

public class HomeStreamEnums
{
    public enum JobStatus
    {
        Queued = 1,
        InProgress,
        Failed,
        Completed,
        CancellationRequested,
        Cancelled
    }
}
