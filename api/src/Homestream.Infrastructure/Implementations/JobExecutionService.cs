using HomeStream.Application.Abstractions.Model;
using HomeStream.Application.Abstractions.Persistence;
using HomeStream.Application.Abstractions.Services;
using HomeStream.Application.Common;
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
        var homestreamDbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHomeStreamDbContext>();
        var queuedJobs = await homestreamDbContext.JobDetails
            .AsQueryable()
            .Where(job => job.Status == JobStatus.Queued && !_activeJobTracker.Contains(job.Id))
            .OrderBy(job => job.CreatedOn)
            .Take(simultaneousJobCount)
            .ToListAsync(cancellationToken);

        foreach (var queuedJob in queuedJobs)
        {
            try
            {
                Type payloadType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .First(t => t.Name == queuedJob.JobPayloadTypeName);

                IJobPayload? payload = JsonConvert.DeserializeObject(queuedJob.Payload, payloadType) as IJobPayload;

                Type jobType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .First(t => t.Name == queuedJob.JobTypeName);

                IJob? job = _serviceProvider.CreateScope().ServiceProvider.GetService(jobType) as IJob;

                _activeJobTracker.Add(queuedJob.Id, cancellationToken);

                _ = Task.Run(async () =>
                {
                    _ = await job.ExecuteJob(payload, queuedJob.Id);
                }, cancellationToken);

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
    }

    public async Task<bool> CancelJob(long jobId, CancellationToken cancellationToken)
    {
        var homestreamDbContext = (HomeStreamDbContext)_serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHomeStreamDbContext>();
        var queuedJob = await homestreamDbContext.JobDetails
            .AsQueryable()
            .FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken);

        if (_activeJobTracker.Contains(jobId))
        {
            Type payloadType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .First(t => t.Name == queuedJob.JobPayloadTypeName);
            IJobPayload? payload = JsonConvert.DeserializeObject(queuedJob.Payload, payloadType) as IJobPayload;

            payload.CancellationTokenSource.Cancel();
            _logger.LogInformation("Job: {jobName} with id: {jobId} cancelled", queuedJob.Name, jobId);
            //queuedJob.Status = JobStatus.Cancelled;
            //homestreamDbContext.Update(queuedJob);
            //await homestreamDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        return false;
    }

    public Task UpdateJobProgress(long jobId, double progress)
    {
        throw new NotImplementedException();
    }

    public Task UpdateJobStatus(long jobId, JobStatus updatedStatus)
    {
        throw new NotImplementedException();
    }
}
