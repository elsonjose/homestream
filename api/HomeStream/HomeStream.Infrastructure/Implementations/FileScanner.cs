using HomeStream.Application.Abstractions;
using HomeStream.Application.Common;
using HomeStream.Application.Models;
using Microsoft.Extensions.Logging;

namespace HomeStream.Infrastructure.Implementations;

/// <summary>
/// Defines the class to scan for files in the device.
/// </summary>
public class FileScanner : IFileScanner
{
    /// <inheritdoc/>
    private readonly ILogger<FileScanner> _logger;

    /// <summary>
    /// Initializes a new instance of <seealso cref="FileScanner"/>
    /// </summary>
    /// <param name="logger"></param>
    public FileScanner(ILogger<FileScanner> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc>/>
    public List<FileScannerResult> ScanForVideos(string directoryPath)
    {
        var videoFiles = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                                  .Where(file => HomeStreamConstants.AllowedVideoExtenstions.Contains(Path.GetExtension(file).ToLower()))
                                  .Select(x => new FileScannerResult(x))
                                  .ToList();

        return videoFiles;
    }
}
