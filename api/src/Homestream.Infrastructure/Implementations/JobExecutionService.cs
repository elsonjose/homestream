using HomeStream.Application.Abstractions.Persistence;
using HomeStream.Application.Common;
using HomeStream.Domain.Abstractions.Model;
using HomeStream.Domain.Abstractions.Services;
using HomeStream.Domain.Entities;
using HomeStream.Infrastructure.Persistence.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using static HomeStream.Domain.Core.HomeStreamEnums;

namespace HomeStream.Infrastructure.Implementations;

public class JobExecutionService : IJobExecutionService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobExecutionService> _logger;
    private readonly BlockingCollection<long> _activeJobTracker;
    private readonly int simultaneousJobCount = 10;

    public JobExecutionService(IServiceProvider serviceProvider, ILogger<JobExecutionService> logger)
    {
        _serviceProvider = serviceProvider;
        _activeJobTracker = [];
        _logger = logger;
    }

    public async Task<long> EnqueueJobAsync<TJob, TJobPayload>(TJobPayload payload, string jobName = "")
        where TJob : IJob
        where TJobPayload : IJobPayload
    {
        var payloadStr = JsonConvert.SerializeObject(payload);
        var jobDetails = new JobDetail()
        {
            Name = string.IsNullOrEmpty(jobName) ? typeof(TJob).Name.ToHumanTitleCase() : jobName,
            JobTypeName = typeof(TJob).Name,
            JobPayloadTypeName = typeof(TJobPayload).Name,
            Payload = payloadStr,
            CreatedOn = HomeStreamDateTime.EpochNow,
        };

        var homestreamDbContext = (HomeStreamDbContext)_serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHomeStreamDbContext>();
        await homestreamDbContext.JobDetails.AddAsync(jobDetails);
        await homestreamDbContext.SaveChangesAsync();
        return jobDetails.Id;
    }

    public async Task StartJobExecution(CancellationToken cancellationToken)
    {
        IHomeStreamDbContext homestreamDbContext = GetNewDbContextInstance();

        var queuedJobs = await homestreamDbContext.JobDetails
            .AsQueryable()
            .Where(job => job.Status == JobStatus.Queued && !_activeJobTracker.Contains(job.Id))
            .OrderBy(job => job.CreatedOn)
            .Take(simultaneousJobCount)
            .ToListAsync(cancellationToken);

        foreach (var queuedJob in queuedJobs)
        {
            Type jobType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .First(t => t.Name == queuedJob.JobTypeName);

            IJob job = (IJob)_serviceProvider.CreateScope().ServiceProvider.GetRequiredService(jobType);

            _activeJobTracker.Add(queuedJob.Id, cancellationToken);

            _ = Task.Run(async () =>
            {
                _ = await job.ExecuteJob(queuedJob.Payload, queuedJob.Id);
            }, cancellationToken);
        }
    }

    public async Task<bool> CancelJob(long jobId, CancellationToken cancellationToken)
    {
        var homestreamDbContext = GetNewDbContextInstance();
        JobDetail queuedJob = await GetQueuedJobDetails(jobId, homestreamDbContext, cancellationToken);

        if (_activeJobTracker.Contains(jobId))
        {
            queuedJob.IsCancellationRequested = true;
            queuedJob.Status = JobStatus.CancellationRequested;
            queuedJob.CancellationRequestOn = HomeStreamDateTime.EpochNow;
            homestreamDbContext.Modify(queuedJob);
            await homestreamDbContext.PersistChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Cancellation required for job {jobName} with id {jobId} at {datetime}", queuedJob.Name, jobId, queuedJob.CancellationRequestOn.Value.ToDateTimeString());

            // TODO: Remove item from blocking collection.

            return true;
        }
        else
        {
            _logger.LogInformation("Failed to cancel job with id {jobId}", jobId);
        }
        return false;
    }

    public async Task UpdateJobProgress(long jobId, double progress, CancellationToken cancellationToken)
    {
        var homestreamDbContext = GetNewDbContextInstance();
        JobDetail queuedJob = await GetQueuedJobDetails(jobId, homestreamDbContext, cancellationToken);
        queuedJob.Progress = progress;
        homestreamDbContext.Modify(queuedJob);
        await homestreamDbContext.PersistChangesAsync(cancellationToken);
    }

    public async Task UpdateJobStatus(long jobId, JobStatus updatedStatus, CancellationToken cancellationToken)
    {
        var homestreamDbContext = GetNewDbContextInstance();
        JobDetail queuedJob = await GetQueuedJobDetails(jobId, homestreamDbContext, cancellationToken);
        queuedJob.Status = updatedStatus;
        homestreamDbContext.Modify(queuedJob);
        await homestreamDbContext.PersistChangesAsync(cancellationToken);
    }
    
    private IHomeStreamDbContext GetNewDbContextInstance()
    {
        return _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHomeStreamDbContext>();
    }

    private static async Task<JobDetail> GetQueuedJobDetails(long jobId, IHomeStreamDbContext homestreamDbContext, CancellationToken cancellationToken)
    {
        return await homestreamDbContext.JobDetails
                    .AsQueryable()
                    .FirstAsync(j => j.Id == jobId, cancellationToken);
    }
}