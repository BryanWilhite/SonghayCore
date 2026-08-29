namespace Songhay.S3.Activities;

/// <summary>
/// Deletes the <see cref="S3Object"/>
/// with the specified <see cref="S3Object.Key"/>.
/// </summary>
public class AmazonS3DeleteS3ObjectActivity(ProgramMetadata programMetadata, ILogger<AmazonS3DeleteS3ObjectActivity>? logger) :
    IActivityTask<(string setKey, string bucketMetaKey, string bucketKey)>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task StartAsync((string setKey, string bucketMetaKey, string bucketKey) input)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        var(setKey, bucketMetaKey, bucketKey) = input;

        RestApiMetadata? s3Meta = programMetadata.RestApiMetadataSet.TryGetValueWithKey(setKey);

        string? bucketName = null;

        AmazonS3Client? s3Client = AmazonS3Utility
            .GetAmazonS3Client(s3Meta, bucketMetaKey, nameof(AmazonS3DeleteS3ObjectActivity),
                t =>
                {
                    var (credentialsProfileName, bN, region, uriRoot) = t;

                    bucketName = bN;

                    logger.LogDebug("{Name}: {Value}", nameof(credentialsProfileName), credentialsProfileName);
                    logger.LogDebug("{Name}: {Value}", nameof(region), region);
                    logger.LogDebug("{Name}: {Value}", nameof(bucketName), bucketName);
                    logger.LogDebug("{Name}: {Value}", nameof(uriRoot), uriRoot);
                    
                }, logger);

        if (s3Client == null)
        {
            logger.LogErrorForMissingData<AmazonS3Client>();

            return;
        }

        DeleteObjectRequest request = new()
        {
            BucketName = bucketName,
            Key = bucketKey
        };

        DeleteObjectResponse response = await s3Client.DeleteObjectAsync(request).ConfigureAwait(false);

        if (response.HttpStatusCode != HttpStatusCode.NoContent)
        {
            logger.LogError("The expected {Name} is not here: {Value}. Returning...", nameof(HttpStatusCode), response.HttpStatusCode);
        }
    }
}
