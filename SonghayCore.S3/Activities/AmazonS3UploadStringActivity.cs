namespace Songhay.S3.Activities;

/// <summary>
/// Uploads the specified <see cref="S3Object"/>
/// to the specified <see cref="S3Bucket"/>.
/// </summary>
public class AmazonS3UploadStringActivity(ProgramMetadata programMetadata, ILogger<AmazonS3UploadStringActivity>? logger):
    IActivityTask<(string setKey, string bucketMetaKey, string bucketKey, string content, string contentMimeType)>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task StartAsync((string setKey, string bucketMetaKey, string bucketKey, string content, string contentMimeType) input)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        var(setKey, bucketMetaKey, bucketKey, content, contentMimeType) = input;

        RestApiMetadata? s3Meta = programMetadata.RestApiMetadataSet.TryGetValueWithKey(setKey);

        AmazonS3Client? s3Client = AmazonS3Utility.GetAmazonS3Client(
            s3Meta,
            bucketMetaKey,
            nameof(AmazonS3UploadStringActivity),
            environmentVariableTarget: null,
            out string? bucketName,
            logger);

        if (s3Client == null)
        {
            logger.LogErrorForMissingData<AmazonS3Client>();

            return;
        }

        PutObjectRequest request = new()
        {
            BucketName = bucketName,
            Key = bucketKey,
            ContentBody = content,
            ContentType = contentMimeType
        };

        PutObjectResponse response = await s3Client.PutObjectAsync(request).ConfigureAwait(false);

        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            logger.LogError("The expected {Name} is not here: {Value}. Returning...", nameof(HttpStatusCode), response.HttpStatusCode);
        }
    }
}
