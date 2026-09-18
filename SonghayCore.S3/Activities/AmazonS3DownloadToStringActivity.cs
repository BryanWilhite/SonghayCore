namespace Songhay.S3.Activities;

/// <summary>
/// Downloads the <see cref="S3Object"/>
/// with the specified <see cref="S3Object.Key"/>.
/// </summary>
public class AmazonS3DownloadToStringActivity(ProgramMetadata programMetadata, ILogger<AmazonS3DownloadToStringActivity>? logger) :
    IActivityTask<StorageActivityInput?, StorageActivityResult<string?>?>
{
    /// <inheritdoc/>
    public async Task<StorageActivityResult<string?>?> StartAsync(StorageActivityInput? input, CancellationToken cancellationToken)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        if (input == null)
        {
            return new StorageActivityResult<string?>(
                HttpStatusCode.BadRequest,
                null,
                "The expected input is not here.",
                null);
        }

        RestApiMetadata? s3Meta = programMetadata.RestApiMetadataSet.GetValueWithKey(input.SetKey);

        AmazonS3Client? s3Client = AmazonS3Utility.GetAmazonS3Client(
            s3Meta,
            input.BucketMetaKey,
            nameof(AmazonS3DownloadToStringActivity),
            out string? bucketName,
            logger);

        if (s3Client == null)
        {
            logger.LogErrorForMissingData<AmazonS3Client>();

            return new StorageActivityResult<string?>(
                HttpStatusCode.InternalServerError,
                null,
                "The expected S3 Client is not here.",
                null);
        }

        GetObjectRequest request = new() { BucketName = bucketName, Key = input.BucketKeyOrPrefix };

        using GetObjectResponse response = await s3Client.GetObjectAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            logger.LogError("The expected {Name} is not here: {Value}. Returning...", nameof(HttpStatusCode), response.HttpStatusCode);

            return new StorageActivityResult<string?>(
                response.HttpStatusCode,
                response.ResponseMetadata.RequestId,
                $"Uncertain whether item {input.BucketKeyOrPrefix} found.",
                null);
        }

        if (response.ContentLength <= 0)
        {
            logger.LogError("The expected {Name} is not here: {Value}. Returning...", nameof(GetObjectResponse.ContentLength), response.ContentLength);

            return new StorageActivityResult<string?>(
                response.HttpStatusCode,
                response.ResponseMetadata.RequestId,
                $"Uncertain whether item {input.BucketKeyOrPrefix} content found or item is empty.",
                null);
        }

        string? content = await response.ResponseStream.ReadStreamAsStringAsync();

        return new StorageActivityResult<string?>(
            response.HttpStatusCode,
            response.ResponseMetadata.RequestId,
            $"Item {input.BucketKeyOrPrefix} content found.",
            content);
    }
}
