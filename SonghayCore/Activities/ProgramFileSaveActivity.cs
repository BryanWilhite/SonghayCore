namespace Songhay.Activities;

/// <summary>
/// Saves to the local file system,
/// impersonating the input and output
/// of <c>AmazonS3UploadStringActivity</c>
/// </summary>
/// <param name="logger">the <see cref="ILogger"/></param>
public class ProgramFileSaveActivity(ILogger<ProgramFileSaveActivity> logger) : IActivityTask<StorageActivityInput<string?>?, EndpointResult>
{
    /// <inheritdoc/>
    public async Task<EndpointResult> StartAsync(StorageActivityInput<string?>? input, CancellationToken cancellationToken)
    {
        if (input == null || !Directory.Exists(input.SetKey))
        {
            const string message = "The expected root directory is not here.";

            logger.LogError(message);

            return new EndpointResult(
                HttpStatusCode.NotFound,
                "[local]",
                message);
        }

        DirectoryInfo directoryInfo = new(input.SetKey);

        string path = directoryInfo.ToCombinedPath(input.BucketKeyOrPrefix);

        if (string.IsNullOrWhiteSpace(path))
        {
            const string message = "The expected path is not here.";

            logger.LogError(message);

            return new EndpointResult(
                HttpStatusCode.NotFound,
                "[local]",
                message);
        }

        await File.WriteAllTextAsync(path, input.Content, cancellationToken);

        return new EndpointResult(
            HttpStatusCode.OK,
            "[local]",
            $"Content written to `{path}`.");
    }
}
