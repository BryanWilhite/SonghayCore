using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using InputForActivities = OneOf.OneOf<
    (string setKey, string bucketMetaKey),
    (string setKey, string bucketMetaKey, string? bucketKey),
    (string setKey, string bucketMetaKey, string? bucketKey, string? content, string? contentMimeType)>;

namespace SonghayCore.S3.Activities;

public class AmazonS3ShellActivity(IConfiguration? configuration,
        [FromKeyedServices(nameof(AmazonS3DeleteS3ObjectActivity))] IActivityTask<(string setKey, string bucketMetaKey, string bucketKey)> activityForAmazonS3DeleteS3Object,
        [FromKeyedServices(nameof(AmazonS3DownloadToStringActivity))] IActivityTask<(string setKey, string bucketMetaKey, string bucketKey), string?> activityForAmazonS3DownloadToString,
        [FromKeyedServices(nameof(AmazonS3ListBucketObjectsWithPaginationActivity))] IActivityTask<(string setKey, string bucketMetaKey), string?> activityForAmazonS3ListBucketObjectsWithPagination,
        [FromKeyedServices(nameof(AmazonS3UploadStringActivity))] IActivityTask<(string setKey, string bucketMetaKey, string bucketKey, string content, string contentMimeType)> activityForAmazonS3UploadString,
        ILogger<AmazonS3ShellActivity>logger

    ) : IActivityOutputOnlyTask<string?>
{
    public async Task<string?> StartAsync()
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ILoggerUtility.AsInstanceOrNullLogger(logger);

        logger.LogInformation("{ActivityName} starting...", nameof
        (AmazonS3ShellActivity));

        string? setKey = configuration.GetCommandLineArgValue(ArgSetKey);
        setKey.ThrowWhenNullOrWhiteSpace();

        string? bucketMetaKey = configuration.GetCommandLineArgValue(ArgBucketMetaKey);
        bucketMetaKey.ThrowWhenNullOrWhiteSpace();

        string? bucketKey = configuration.GetCommandLineArgValue(ArgBucketKey);
        string? content = configuration.ReadStringInput();
        string? contentMimeType = configuration.GetCommandLineArgValue(ArgBucketS3ObjectMimetype);

        InputForActivities input = (setKey, bucketMetaKey, bucketKey, content, contentMimeType) switch
        {
            (var s1, var s2, null, null, null) => (s1, s2),
            (var s1, var s2, var s3, null, null) => (s1, s2, s3),
            var (s1, s2, s3, s4, s5) => (s1, s2, s3, s4, s5)
        };

        string? activitySetKey = configuration.GetCommandLineArgValue(ConsoleArgsScalars.ActivityName);
        activitySetKey.ThrowWhenNullOrWhiteSpace();

        Func<InputForActivities, Task<string?>>? activity = ActivitySet.TryGetValueWithKey(activitySetKey);

        if (activity == null)
        {
            logger.LogError("The expected Activity, `{Name}`, is not here.", activitySetKey);

            return null;
        }

        return await activity.Invoke(input);
    }

    internal const string ArgSetKey = "--set-key";
    internal const string ArgBucketMetaKey = "--bucket-meta-key";
    internal const string ArgBucketKey = "--bucket-key";
    internal const string ArgBucketS3ObjectMimetype = "--bucket-object-mime-type";

    internal readonly Dictionary<string, Func<InputForActivities, Task<string?>>> ActivitySet = new()
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
        },
    };
}
