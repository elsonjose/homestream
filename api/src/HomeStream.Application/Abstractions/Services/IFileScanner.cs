using HomeStream.Application.Models;

namespace HomeStream.Application.Abstractions.Services;

/// <summary>
/// Defines the interface for file scanner.
/// </summary>
public interface IFileScanner
{
    /// <summary>
    /// Defines the method to scan for videos.
    /// </summary>
    /// <param name="directoryPath">The base directory path</param>
    /// <returns>A list of videos of type <seealso cref="FileScannerResult"/></returns>
    public List<FileScannerResult> ScanForVideos(string directoryPath);
}