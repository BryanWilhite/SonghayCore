namespace Songhay.S3.Activities;

/// <summary>
/// Deletes the <see cref="S3Object"/>
/// with the specified <see cref="S3Object.Key"/>.
/// </summary>
public class AmazonS3DeleteS3ObjectActivity(ProgramMetadata programMetadata, ILogger<AmazonS3DeleteS3ObjectActivity>? logger) :
    IActivityTask<StorageActivityInput?, StorageActivityResult?>
{
    /// <inheritdoc/>
    public async Task<StorageActivityResult?> StartAsync(StorageActivityInput? input, CancellationToken cancellationToken)
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
            nameof(AmazonS3DeleteS3ObjectActivity),
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

        DeleteObjectRequest request = new()
        {
            BucketName = bucketName,
            Key = input.BucketKeyOrPrefix
        };

        DeleteObjectResponse response = await s3Client.DeleteObjectAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.HttpStatusCode == HttpStatusCode.NoContent)
            return new StorageActivityResult(
                response.HttpStatusCode,
                response.ResponseMetadata.RequestId,
                $"Item {input.BucketKeyOrPrefix} deleted.");

        logger.LogError("The expected {Name} is not here: {Value}. Returning...", nameof(HttpStatusCode), response.HttpStatusCode);

        return new StorageActivityResult(
            response.HttpStatusCode,
            response.ResponseMetadata.RequestId,
            $"Uncertain whether item {input.BucketKeyOrPrefix} deleted.");

    }
}
