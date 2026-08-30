using Microsoft.Extensions.DependencyInjection;

using Songhay.S3.Activities;

using InputForActivities = OneOf.OneOf<
    (string setKey, string bucketMetaKey),
    (string setKey, string bucketMetaKey, string? bucketKey),
    (string setKey, string bucketMetaKey, string? bucketKey, string? content, string? contentMimeType)>;

namespace Songhay.S3.Models;

/// <summary>
/// Maps <see cref="Songhay.S3.Activities"/>
/// to their respective inputs
/// based on the convention of using tuples for inputs.
/// </summary>
public class AmazonS3ActivityGroup(
    [FromKeyedServices(nameof(AmazonS3DeleteS3ObjectActivity))] IActivityTask<(string setKey, string bucketMetaKey, string bucketKey)> activityForAmazonS3DeleteS3Object,
    [FromKeyedServices(nameof(AmazonS3DownloadToStringActivity))] IActivityTask<(string setKey, string bucketMetaKey, string bucketKey), string?> activityForAmazonS3DownloadToString,
    [FromKeyedServices(nameof(AmazonS3ListBucketObjectsWithPaginationActivity))] IActivityTask<(string setKey, string bucketMetaKey), string?> activityForAmazonS3ListBucketObjectsWithPagination,
    [FromKeyedServices(nameof(AmazonS3UploadStringActivity))] IActivityTask<(string setKey, string bucketMetaKey, string bucketKey, string content, string contentMimeType)> activityForAmazonS3UploadString,
    ILogger<AmazonS3ActivityGroup>logger
) : IActivityKeyedTaskGroup
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<string?> InvokeActivityAsync(string? activitySetKey, params string?[] args )
    {
        activitySetKey.ThrowWhenNullOrWhiteSpace();

        int expected = 5;

        if (args.Length != expected)
        {
            logger.LogError("The expected number of Activity args ({No}) for `{Name}` is not here.", expected, activitySetKey);

            return null;
        }

        string? setKey = args[0];
        setKey.ThrowWhenNullOrWhiteSpace();

        string? bucketMetaKey = args[1];
        bucketMetaKey.ThrowWhenNullOrWhiteSpace();

        string? bucketKey = args[2];
        string? content = args[3];
        string? contentMimeType = args[4];

        InputForActivities input = (setKey, bucketMetaKey, bucketKey, content, contentMimeType) switch
        {
            (var s1, var s2, null, null, null) => (s1, s2),
            (var s1, var s2, var s3, null, null) => (s1, s2, s3),
            var (s1, s2, s3, s4, s5) => (s1, s2, s3, s4, s5)
        };

        Func<InputForActivities, Task<string?>>? activity = _activitySet.TryGetValueWithKey(activitySetKey);

        if (activity == null)
        {
            logger.LogError("The expected Activity, `{Name}`, is not here.", activitySetKey);

            return null;
        }

        return await activity.Invoke(input);
    }

    private readonly Dictionary<string, Func<InputForActivities, Task<string?>>> _activitySet = new()
    {
        [nameof(AmazonS3DeleteS3ObjectActivity)] = async input =>
        {
            var (setKey, bucketMetaKey, bucketKey) = input.AsT1;

            bucketKey.ThrowWhenNullOrWhiteSpace();

            await activityForAmazonS3DeleteS3Object.StartAsync((setKey, bucketMetaKey, bucketKey));

            return null;
        },
        [nameof(AmazonS3DownloadToStringActivity)] = async input =>
        {
            var (setKey, bucketMetaKey, bucketKey) = input.AsT1;

            bucketKey.ThrowWhenNullOrWhiteSpace();

            string? output = await activityForAmazonS3DownloadToString.StartAsync((setKey, bucketMetaKey, bucketKey));

            return output;
        },
        [nameof(AmazonS3ListBucketObjectsWithPaginationActivity)] = async input =>
        {
            var (setKey, bucketMetaKey) = input.AsT0;

            string? output = await activityForAmazonS3ListBucketObjectsWithPagination.StartAsync((setKey, bucketMetaKey));

            return output;
        },
        [nameof(AmazonS3UploadStringActivity)] = async input =>
        {
            var (setKey, bucketMetaKey, bucketKey, content, contentMimeType) = input.AsT2;

            bucketKey.ThrowWhenNullOrWhiteSpace();
            content.ThrowWhenNullOrWhiteSpace();
            contentMimeType.ThrowWhenNullOrWhiteSpace();

            await activityForAmazonS3UploadString.StartAsync((setKey, bucketMetaKey, bucketKey, content, contentMimeType));

            return null;
        }
    };
}
