using HomeStream.Domain.Abstractions.Model;
using static HomeStream.Domain.Core.HomeStreamEnums;

namespace HomeStream.Domain.Abstractions.Services;

public interface IJobExecutionService
{
    public Task StartJobExecution(CancellationToken cancellationToken);

    public Task<long> EnqueueJobAsync<TJob, TJobPayload>(TJobPayload payload, string jobName = "") where TJob : IJob where TJobPayload : IJobPayload;

    public Task UpdateJobProgress(long jobId, double progress, CancellationToken cancellationToken);

    public Task UpdateJobStatus(long jobId, JobStatus updatedStatus, CancellationToken cancellationToken);

    public Task<bool> CancelJob(long jobId, CancellationToken cancellationToken);
    public Task<bool> RequeueJob(long jobId, CancellationToken cancellationToken);
}