namespace Songhay.S3.Activities;

/// <summary>
/// Uploads the specified <see cref="S3Object"/>
/// to the specified <see cref="S3Bucket"/>.
/// </summary>
public class AmazonS3UploadStringActivity(ProgramMetadata programMetadata, ILogger<AmazonS3UploadStringActivity>? logger):
    IActivityTask<StorageActivityInput<string?>?, StorageActivityResult?>
{
    /// <inheritdoc/>
    public async Task<StorageActivityResult?> StartAsync(StorageActivityInput<string?>? input, CancellationToken cancellationToken)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        if (input == null)
        {
            return new StorageActivityResult(
                HttpStatusCode.BadRequest,
                null,
                "The expected input is not here.");
        }

        RestApiMetadata? s3Meta = programMetadata.RestApiMetadataSet.GetValueWithKey(input.SetKey);

        AmazonS3Client? s3Client = AmazonS3Utility.GetAmazonS3Client(
            s3Meta,
            input.BucketMetaKey,
            nameof(AmazonS3UploadStringActivity),
            out string? bucketName,
            logger);

        if (s3Client == null)
        {
            logger.LogErrorForMissingData<AmazonS3Client>();

            return new StorageActivityResult(
                HttpStatusCode.InternalServerError,
                null,
                "The expected S3 Client is not here.");
        }

        PutObjectRequest request = new()
        {
            BucketName = bucketName,
            Key = input.BucketKeyOrPrefix,
            ContentBody = input.Content,
            ContentType = input.ContentMimeType
        };

        PutObjectResponse response = await s3Client.PutObjectAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.HttpStatusCode == HttpStatusCode.OK)
            return new StorageActivityResult(
                response.HttpStatusCode,
                response.ResponseMetadata.RequestId,
                $"Item {input.BucketKeyOrPrefix} uploaded.");
        logger.LogError("The expected {Name} is not here: {Value}. Returning...", nameof(HttpStatusCode), response.HttpStatusCode);

        return new StorageActivityResult(
            response.HttpStatusCode,
            response.ResponseMetadata.RequestId,
            $"Uncertain whether item {input.BucketKeyOrPrefix} uploaded.");

    }
}
