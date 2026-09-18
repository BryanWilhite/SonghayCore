using System.Text.Json;
using Songhay.S3.Activities;
using Songhay.S3.SerializerContexts;
using InputForActivities = OneOf.OneOf<Songhay.Models.StorageActivityInput, Songhay.Models.StorageActivityInput<string?>>;

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
    IActivityTask<StorageActivityInput?, StorageActivityResult?> activityForAmazonS3DeleteS3Object,
    IActivityTask<StorageActivityInput?, StorageActivityResult<string?>?> activityForAmazonS3DownloadToString,
    IActivityTask<StorageActivityInput?, StorageActivityResult<IReadOnlyCollection<StorageObject>>?> activityForAmazonS3ListBucketObjectsWithPagination,
    IActivityTask<StorageActivityInput<string?>?, StorageActivityResult?> activityForAmazonS3UploadString,
    ILogger<AmazonS3ActivityGroup>logger
) : IActivityKeyedTaskGroup
{
    /// <inheritdoc/>
    public async Task<string?> InvokeActivityAsync(string? activitySetKey, params string?[] args )
    {
        activitySetKey.ThrowWhenNullOrWhiteSpace();

        int minimumExpected = 2;

        if (args.Length < minimumExpected)
        {
            logger.LogError("The minimum expected number of Activity args ({No}) for `{Name}` is not here.", minimumExpected, activitySetKey);

            return null;
        }

        string? setKey = args[0];
        setKey.ThrowWhenNullOrWhiteSpace();

        string? bucketMetaKey = args[1];
        bucketMetaKey.ThrowWhenNullOrWhiteSpace();

        string? bucketKey = args.ElementAtOrDefault(2);
        string? content = args.ElementAtOrDefault(3);
        string? contentMimeType = args.ElementAtOrDefault(4);

        InputForActivities input = (setKey, bucketMetaKey, bucketKey, content, contentMimeType) switch
        {
            (var s1, var s2, var s3, null, null) => new StorageActivityInput(s1, s2, s3),
            var (s1, s2, s3, s4, s5) => new StorageActivityInput<string?>(s1, s2, s3, s4, s5)
        };

        Func<InputForActivities, Task<string?>>? activity = _activitySet.GetValueWithKey(activitySetKey);

        if (activity != null) return await activity.Invoke(input);

        logger.LogError("The expected Activity, `{Name}`, is not here.", activitySetKey);

        return null;

    }

    private static JsonSerializerOptions GetJsonDeserializerOptions()
    {
        JsonSerializerOptions options = new();

        options.TypeInfoResolverChain.Add(StorageObjectSerializerContext.Default);

        return options;
    }

    private readonly Dictionary<string, Func<InputForActivities, Task<string?>>> _activitySet = new()
    {
        [nameof(AmazonS3DeleteS3ObjectActivity)] = async input =>
        {
            StorageActivityResult? result = await activityForAmazonS3DeleteS3Object.StartAsync(input.AsT0, CancellationToken.None);

            return result?.ResponseMessage;
        },
        [nameof(AmazonS3DownloadToStringActivity)] = async input =>
        {
            StorageActivityResult<string?>? result = await activityForAmazonS3DownloadToString.StartAsync(input.AsT0, CancellationToken.None);

            return result?.Content;
        },
        [nameof(AmazonS3ListBucketObjectsWithPaginationActivity)] = async input =>
        {
            StorageActivityResult<IReadOnlyCollection<StorageObject>>? result = await activityForAmazonS3ListBucketObjectsWithPagination.StartAsync(input.AsT0, CancellationToken.None);

            if (result?.Content == null) return result?.ResponseMessage;

            string json = JsonSerializer.Serialize(result.Content, GetJsonDeserializerOptions());

            return json;
        },
        [nameof(AmazonS3UploadStringActivity)] = async input =>
        {
            StorageActivityResult? result = await activityForAmazonS3UploadString.StartAsync(input.AsT1, CancellationToken.None);

            return result?.ResponseMessage;
        }
    };
}
