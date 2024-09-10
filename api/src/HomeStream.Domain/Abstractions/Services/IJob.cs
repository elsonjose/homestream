namespace HomeStream.Domain.Abstractions.Services;

public interface IJob
{
    public Task ExecuteJob(string serializedPayload, long jobId);
}