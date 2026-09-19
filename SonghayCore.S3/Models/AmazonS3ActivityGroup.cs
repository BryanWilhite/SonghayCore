using Songhay.S3.Activities;

namespace Songhay.S3.Models;

/// <summary>
/// Maps typical string args into tuples
/// into one of the respective <see cref="Songhay.S3.Activities"/>.
/// </summary>
/// <param name="activityForAmazonS3DeleteS3Object">the S3 delete Activity</param>
/// <param name="activityForAmazonS3DownloadToString">the S3 string-download Activity</param>
/// <param name="activityForAmazonS3ListBucketObjectsWithPagination">the S3 bucket-list Activity</param>
/// <param name="activityForAmazonS3UploadString">the S3 string-upload Activity</param>
/// <param name="logger">the <see cref="ILogger"/></param>
public class AmazonS3ActivityGroup(
    IActivityTask<StorageActivityInput?, EndpointResult> activityForAmazonS3DeleteS3Object,
    IActivityTask<StorageActivityInput?, EndpointContentResult<string?>> activityForAmazonS3DownloadToString,
    IActivityTask<StorageActivityInput?, EndpointContentResult<IReadOnlyCollection<StorageObject>>> activityForAmazonS3ListBucketObjectsWithPagination,
    IActivityTask<StorageActivityInput<string?>?, EndpointResult> activityForAmazonS3UploadString,
    ILogger<AmazonS3ActivityGroup>logger
) : IActivityKeyedTaskGroup<EndpointResult>
{
    /// <inheritdoc/>
    public async Task<EndpointResult> InvokeActivityAsync(string? activitySetKey, CancellationToken cancellationToken, params string?[] args)
    {
        activitySetKey.ThrowWhenNullOrWhiteSpace();

        const int minimumExpected = 2;

        if (args.Length < minimumExpected)
        {
            logger.LogError("The minimum expected number of Activity args ({No}) for `{Name}` is not here.", minimumExpected, activitySetKey);

            return new EndpointResult(
                HttpStatusCode.InternalServerError,
                null,
                "Arguments were not valid.");
        }

        string? setKey = args[0];
        setKey.ThrowWhenNullOrWhiteSpace();

        string? bucketMetaKey = args[1];
        bucketMetaKey.ThrowWhenNullOrWhiteSpace();

        string? bucketKey = args.ElementAtOrDefault(2);
        string? content = args.ElementAtOrDefault(3);
        string? contentMimeType = args.ElementAtOrDefault(4);

        var activity = _activitySet.GetValueWithKey(activitySetKey);

        StorageActivityInput input = string.IsNullOrWhiteSpace(content) ?
            new StorageActivityInput(setKey, bucketMetaKey, bucketKey)
            :
            new StorageActivityInput<string?>(setKey, bucketMetaKey, bucketKey, content, contentMimeType);

        if (activity != null) return await activity.Invoke(input, cancellationToken);

        logger.LogError("The expected Activity, `{Name}`, is not here.", activitySetKey);

        return new EndpointResult(
            HttpStatusCode.InternalServerError,
            null,
            "Activity was not found.");
    }

    private readonly Dictionary<string, Func<StorageActivityInput, CancellationToken, Task<EndpointResult>>> _activitySet = new()
    {
        [nameof(AmazonS3DeleteS3ObjectActivity)] = async (input, token) =>
        {
            EndpointResult result = await activityForAmazonS3DeleteS3Object.StartAsync(input, token);

            return result;
        },
        [nameof(AmazonS3DownloadToStringActivity)] = async (input, token) =>
        {
            EndpointContentResult<string?> result = await activityForAmazonS3DownloadToString.StartAsync(input, token);

            return result;
        },
        [nameof(AmazonS3ListBucketObjectsWithPaginationActivity)] = async (input, token) =>
        {
            EndpointContentResult<IReadOnlyCollection<StorageObject>> result = await activityForAmazonS3ListBucketObjectsWithPagination.StartAsync(input, token);

            return result;
        },
        [nameof(AmazonS3UploadStringActivity)] = async (input, token) =>
        {
            EndpointResult result = await activityForAmazonS3UploadString.StartAsync(input as StorageActivityInput<string?>, token);

            return result;
        }
    };
}
