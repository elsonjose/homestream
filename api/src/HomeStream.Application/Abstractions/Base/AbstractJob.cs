using HomeStream.Application.Abstractions.Persistence;
using HomeStream.Application.Common;
using HomeStream.Domain.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static HomeStream.Domain.Core.HomeStreamEnums;

namespace HomeStream.Application.Abstractions;

public abstract class AbstractJob : IJob
{
    protected readonly IServiceProvider _serviceProvider;
    protected readonly IJobExecutionService _jobExecutionService;
    protected readonly ICacheService _redisCacheService;
    protected readonly ILogger<AbstractJob> _logger;

    public AbstractJob(IServiceProvider serviceProvider,
        IJobExecutionService jobExecutionService,
        ICacheService redisCacheService,
        ILogger<AbstractJob> logger)
    {
        _serviceProvider = serviceProvider;
        _jobExecutionService = jobExecutionService;
        _redisCacheService = redisCacheService;
        _logger = logger;
    }

    protected abstract Task Invoke(string seralizedPayload, CancellationToken cancellationToken = default);

    public async Task ExecuteJob(string serializedPayload, long jobId)
    {
        string redisKeyForJob = string.Format(HomeStreamConstants.RedisCacheKeyForJob, jobId);
        var isKeyInCache = await _redisCacheService.IsKeyInCache(redisKeyForJob);

        if (!isKeyInCache)
        {
            await _redisCacheService.SetCacheItemAsync(redisKeyForJob, jobId);

            var dbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHomeStreamDbContext>();

            var jobDetails = await dbContext.JobDetails.AsQueryable().FirstAsync(j => j.Id == jobId, CancellationToken.None);
            jobDetails.StartedOn = HomeStreamDateTime.EpochNow;

            await _jobExecutionService.UpdateJobStatus(jobId, JobStatus.InProgress, CancellationToken.None);

            try
            {
                _logger.LogInformation(
                    "Started execution of job with id: {jobId} at {time} with payload:\n{paylod} a", jobId, jobDetails.StartedOn.Value.ToDateTimeString(), serializedPayload);

                await Invoke(serializedPayload, CancellationToken.None);

                jobDetails.Status = JobStatus.Completed;
                jobDetails.Progress = 100;
            }
            catch (Exception ex)
            {
                jobDetails.ExceptionMessage = ex.Message;
                jobDetails.StackTrace = string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : ex.StackTrace;
                jobDetails.Status = JobStatus.Failed;
            }
            finally
            {
                jobDetails.CompletedOn = HomeStreamDateTime.EpochNow;
                _logger.LogInformation("Ended execution of job with id: {jobId} at {time}", jobId, jobDetails.CompletedOn.Value.ToDateTimeString());

                var duration = TimeSpan.FromSeconds(jobDetails.CompletedOn.Value - jobDetails.StartedOn.Value);
                _logger.LogInformation("Total duration of job with id: {jobId} is {duration}", jobId, duration.ToHumanReadableTimeSpan());

                dbContext.Modify(jobDetails);
                await dbContext.PersistChangesAsync(CancellationToken.None);

                await _redisCacheService.RemoveCachedItemAsync(redisKeyForJob);
            }
        }
        else
        {
            _logger.LogInformation("Job with id: {jobId} is locked by cache", jobId);
        }
    }
}