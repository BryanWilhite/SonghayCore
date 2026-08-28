using System.Text.Json;

namespace SonghayCore.S3.Activities;

public class AmazonS3ListBucketObjectsWithPaginationActivity(ProgramMetadata programMetadata, ILogger<AmazonS3ListBucketObjectsWithPaginationActivity>? logger):
    IActivityTask<(string setKey, string bucketMetaKey), string?>
{
    public async Task<string?> StartAsync((string setKey, string bucketMetaKey) input)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        var(setKey, bucketMetaKey) = input;

        RestApiMetadata? s3Meta = programMetadata.RestApiMetadataSet.TryGetValueWithKey(setKey);

        string? bucketName = null;

        AmazonS3Client? s3Client = S3Utility
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

        ListObjectsV2Request request = new()
        {
            BucketName = bucketName,
            Prefix = string.Empty,
            MaxKeys = 10
        };

        IReadOnlyCollection<S3Object> s3Objects = await S3Utility.CollectS3ObjectsFromPaginationAsync(s3Client, request, logger);

        if (s3Objects.Count <= 0)
        {
            logger.LogWarning("No S3 objects found. Returning...");

            return null;
        }

        string json = JsonSerializer.Serialize(s3Objects, JsonSerializerOptions);

        return json;
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        IndentSize = 4,
        WriteIndented = true
    };
}
