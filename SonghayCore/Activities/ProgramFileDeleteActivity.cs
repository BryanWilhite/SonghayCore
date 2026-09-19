namespace Songhay.Activities;

/// <summary>
/// Deletes from the local file system,
/// impersonating the input and output
/// of <c>AmazonS3DeleteS3ObjectActivity</c>
/// </summary>
/// <param name="logger">the <see cref="ILogger"/></param>
public class ProgramFileDeleteActivity(ILogger<ProgramFileDeleteActivity> logger) : IActivityTask<StorageActivityInput?, EndpointResult>
{
    /// <inheritdoc/>
    public async Task<EndpointResult> StartAsync(StorageActivityInput? input, CancellationToken cancellationToken)
    {
        FileInfo? fileInfo = input.ToFileInfo(logger);

        if (fileInfo == null)
        {
            return new EndpointResult(
                HttpStatusCode.NotFound,
                "[local]",
                "The expected file is not here.");
        }

        fileInfo.Delete();

        await Task.CompletedTask;

        return new EndpointResult(
            HttpStatusCode.NoContent,
            "[local]",
            $"File `{fileInfo.FullName}` deleted.");
    }
}
