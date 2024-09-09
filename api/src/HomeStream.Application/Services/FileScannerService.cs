using HomeStream.Domain.Abstractions.Model;
using HomeStream.Domain.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace HomeStream.Application.Implementations.Services;

public class FileScannerServicePayload : IJobPayload
{
    public string Path { get; set; }
    public CancellationTokenSource CancellationTokenSource { get; set; }
}

public class FileScannerService : IJob
{
    private readonly ILogger<FileScannerService> _logger;

    public FileScannerService(ILogger<FileScannerService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> ExecuteJob(string serializedPayload, long jobId)
    {

        //var videoFiles = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
        //                          .Where(file => HomeStreamConstants.AllowedVideoExtenstions.Contains(Path.GetExtension(file).ToLower()))
        //                          .Select(x => new FileScannerResult(x))
        //                          .ToList();
        _logger.LogInformation("\nInside FileScannerService ExecuteJob with jobId = {jobId}\n", jobId);
        await Task.Delay(10000);
        return true;
    }
}
