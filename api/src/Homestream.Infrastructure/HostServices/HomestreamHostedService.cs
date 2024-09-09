using HomeStream.Application.Common;
using HomeStream.Domain.Abstractions.Services;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace HomeStream.Infrastructure.HostServices;

public class HomestreamHostedService : BackgroundService
{
    private readonly IJobExecutionService _jobExecutionService;
    private readonly BlockingCollection<long> _activeJobCollection;

    public HomestreamHostedService(IJobExecutionService jobExecutionService)
    {
        _jobExecutionService = jobExecutionService;
        _activeJobCollection = [];
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _jobExecutionService.StartJobExecution(stoppingToken);
            await Task.Delay(HomeStreamConstants.JobExecutionDelay, stoppingToken);
        }
    }
}
