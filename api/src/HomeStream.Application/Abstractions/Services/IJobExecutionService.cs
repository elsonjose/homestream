using HomeStream.Application.Abstractions.Model;

namespace HomeStream.Application.Abstractions.Services;

public interface IJobExecutionService
{
    public Task StartJobExecution(CancellationToken cancellationToken);

    public Task<long> EnqueueJobAsync<TJob, TJobPayload>(TJobPayload payload, string jobName = "") where TJob : IJob where TJobPayload : IJobPayload;

    public Task UpdateJobProgress(long jobId, double progress);

    public Task<bool> CancelJob(long jobId, CancellationToken cancellationToken);
}
