using HomeStream.Application.Abstractions.Model;

namespace HomeStream.Application.Abstractions.Services;

public interface IJob
{
    public Task<bool> ExecuteJob(IJobPayload payload, long jobId);
}