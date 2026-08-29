namespace SonghayCore.S3.Activities;

public class AmazonS3UploadStringActivity(ProgramMetadata programMetadata, ILogger<AmazonS3ListBucketObjectsWithPaginationActivity>? logger):
    IActivityTask<(string setKey, string bucketMetaKey, string bucketKey, string content, string contentMimeType)>
{
    public async Task StartAsync((string setKey, string bucketMetaKey, string bucketKey, string content, string contentMimeType) input)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        var(setKey, bucketMetaKey, bucketKey, content, contentMimeType) = input;

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
