namespace Songhay.Activities;

/// <summary>
/// Reads a string-content file
/// from the local file system,
/// impersonating the input and output
/// of <c>AmazonS3DownloadToStringActivity</c>
/// </summary>
/// <param name="logger">the <see cref="ILogger"/></param>
public class ProgramFileReadActivity(ILogger<ProgramFileReadActivity> logger) : IActivityTask<StorageActivityInput?, StorageActivityResult<string?>?>
{
    /// <inheritdoc/>
    public async Task<StorageActivityResult<string?>?> StartAsync(StorageActivityInput? input)
    {
        FileInfo? fileInfo = input.ToFileInfo(logger);

        if (fileInfo == null)
        {
            return new StorageActivityResult<string?>(
                HttpStatusCode.NotFound,
                "[local]",
                "The expected file is not here.",
                null);
        }

        string content = await File.ReadAllTextAsync(fileInfo.FullName);

        return new StorageActivityResult<string?>(
            HttpStatusCode.OK,
            "[local]",
            $"File `{fileInfo.FullName}` loaded.",
            content);
    }
}
