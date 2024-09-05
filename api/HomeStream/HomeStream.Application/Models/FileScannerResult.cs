namespace HomeStream.Application.Models;

/// <summary>
/// Defines the model for file scanner result.
/// </summary>
public class FileScannerResult
{
    /// <summary>
    /// Specifies the file path of the result.
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// Specifies the path of parent directory of the result.
    /// </summary>
    public string ParentDirectory { get; set; }

    public FileScannerResult(string filePath)
    {
        FilePath = filePath;
        ParentDirectory = Directory.GetParent(filePath)?.FullName;
    }

}
