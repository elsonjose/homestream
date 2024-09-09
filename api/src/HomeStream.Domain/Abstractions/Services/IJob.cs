namespace HomeStream.Domain.Abstractions.Services;

public interface IJob
{
    public Task<bool> ExecuteJob(string serializedPayload, long jobId);
}