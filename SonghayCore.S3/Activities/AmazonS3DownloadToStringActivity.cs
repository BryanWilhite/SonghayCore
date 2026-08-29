namespace Songhay.S3.Activities;

/// <summary>
/// Downloads the <see cref="S3Object"/>
/// with the specified <see cref="S3Object.Key"/>.
/// </summary>
public class AmazonS3DownloadToStringActivity(ProgramMetadata programMetadata, ILogger<AmazonS3DownloadToStringActivity>? logger) :
    IActivityTask<(string setKey, string bucketMetaKey, string bucketKey), string?>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<string?> StartAsync((string setKey, string bucketMetaKey, string bucketKey) input)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        var(setKey, bucketMetaKey, bucketKey) = input;

        RestApiMetadata? s3Meta = programMetadata.RestApiMetadataSet.TryGetValueWithKey(setKey);

        string? bucketName = null;

        AmazonS3Client? s3Client = AmazonS3Utility
            .GetAmazonS3Client(s3Meta, bucketMetaKey, nameof(AmazonS3DownloadToStringActivity),
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

            return null;
        }

        GetObjectRequest request = new() { BucketName = bucketName, Key = bucketKey };

        using GetObjectResponse response = await s3Client.GetObjectAsync(request).ConfigureAwait(false);

        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            logger.LogError("The expected {Name} is not here: {Value}. Returning...", nameof(HttpStatusCode), response.HttpStatusCode);

            return null;
        }

        if (response.ContentLength <= 0)
        {
            logger.LogError("The expected {Name} is not here: {Value}. Returning...", nameof(GetObjectResponse.ContentLength), response.ContentLength);

            return null;
        }

        return await response.ResponseStream.ReadStreamAsStringAsync();
    }
}
