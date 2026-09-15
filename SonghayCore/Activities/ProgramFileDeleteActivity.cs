namespace Songhay.Activities;

/// <summary>
/// Deletes from the local file system,
/// impersonating the input and output
/// of <c>AmazonS3DeleteS3ObjectActivity</c>
/// </summary>
/// <param name="logger">the <see cref="ILogger"/></param>
public class ProgramFileDeleteActivity(ILogger<ProgramFileDeleteActivity> logger) : IActivityTask<StorageActivityInput?, StorageActivityResult?>
{
    /// <inheritdoc/>
    public async Task<StorageActivityResult?> StartAsync(StorageActivityInput? input, CancellationToken cancellationToken)
    {
        FileInfo? fileInfo = input.ToFileInfo(logger);

        if (fileInfo == null)
        {
            return new StorageActivityResult(
                HttpStatusCode.NotFound,
                "[local]",
                "The expected file is not here.");
        }

        fileInfo.Delete();

        await Task.CompletedTask;

        return new StorageActivityResult(
            HttpStatusCode.NoContent,
            "[local]",
            $"File `{fileInfo.FullName}` deleted.");
    }
}
