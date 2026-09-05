using System.Text.Json;

namespace Songhay.S3.Activities;

/// <summary>
/// Retrieves a JSON-array of serialized <see cref="S3Object"/>
/// of the specified <see cref="S3Bucket"/>.
/// </summary>
public class AmazonS3ListBucketObjectsWithPaginationActivity(ProgramMetadata programMetadata, ILogger<AmazonS3ListBucketObjectsWithPaginationActivity>? logger):
    IActivityTask<(string setKey, string bucketMetaKey, string? bucketKeyPrefix), string?>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<string?> StartAsync((string setKey, string bucketMetaKey, string? bucketKeyPrefix) input)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        var(setKey, bucketMetaKey, bucketKeyPrefix) = input;

        RestApiMetadata? s3Meta = programMetadata.RestApiMetadataSet.TryGetValueWithKey(setKey);

        AmazonS3Client? s3Client = AmazonS3Utility.GetAmazonS3Client(
            s3Meta,
            bucketMetaKey,
            nameof(AmazonS3ListBucketObjectsWithPaginationActivity),
            environmentVariableTarget: null,
            out string? bucketName,
            logger);

        if (s3Client == null)
        {
            logger.LogErrorForMissingData<AmazonS3Client>();

            return null;
        }

        ListObjectsV2Request request = new()
        {
            BucketName = bucketName,
            Prefix = bucketKeyPrefix ?? string.Empty,
            MaxKeys = 10
        };

        IReadOnlyCollection<S3Object> s3Objects = await AmazonS3Utility.CollectS3ObjectsFromPaginationAsync(s3Client, request, logger);

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
