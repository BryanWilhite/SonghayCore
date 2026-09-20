namespace Songhay.S3.Activities;

/// <summary>
/// Retrieves a JSON-array of serialized <see cref="S3Object"/>
/// of the specified <see cref="S3Bucket"/>.
/// </summary>
public class AmazonS3ListBucketObjectsWithPaginationActivity(ProgramMetadata programMetadata, ILogger<AmazonS3ListBucketObjectsWithPaginationActivity>? logger):
    IActivityTask<StorageActivityInput?, EndpointContentResult<IReadOnlyCollection<StorageObject>>>
{
    /// <inheritdoc/>
    public async Task<EndpointContentResult<IReadOnlyCollection<StorageObject>>> StartAsync(StorageActivityInput? input, CancellationToken cancellationToken)
    {
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        if (input == null)
        {
            return new EndpointContentResult<IReadOnlyCollection<StorageObject>>(
                HttpStatusCode.BadRequest,
                null,
                "The expected input is not here.",
                []);
        }

        RestApiMetadata? s3Meta = programMetadata.RestApiMetadataSet.GetValueWithKey(input.SetKey);

        AmazonS3Client? s3Client = AmazonS3Utility.GetAmazonS3Client(
            s3Meta,
            input.BucketMetaKey,
            nameof(AmazonS3ListBucketObjectsWithPaginationActivity),
            out string? bucketName,
            logger);

        if (s3Client == null)
        {
            logger.LogErrorForMissingData<AmazonS3Client>();

            return new EndpointContentResult<IReadOnlyCollection<StorageObject>>(
                HttpStatusCode.InternalServerError,
                null,
                "The expected S3 Client is not here.",
                []);
        }

        ListObjectsV2Request request = new()
        {
            BucketName = bucketName,
            Prefix = input.BucketKeyOrPrefix ?? string.Empty,
            MaxKeys = 10
        };

        EndpointContentResult<IReadOnlyCollection<StorageObject>> result = await AmazonS3Utility.CollectS3ObjectsFromPaginationAsync(s3Client, request, logger);

        return result;
    }
}
