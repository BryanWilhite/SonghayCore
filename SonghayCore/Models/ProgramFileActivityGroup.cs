using Songhay.Activities;

namespace Songhay.Models;

/// <summary>
/// Maps typical string args into tuples
/// into one of the respective <see cref="Songhay.Activities"/>
/// for Program files.
/// </summary>
/// <param name="activityForProgramFileDelete">the Program-file delete Activity</param>
/// <param name="activityForProgramFileRead">the Program-file string-download Activity</param>
/// <param name="activityForProgramFileList">the Program-file bucket-list Activity</param>
/// <param name="activityForProgramFileSave">the Program-file string-upload Activity</param>
/// <param name="logger">the <see cref="ILogger"/></param>
public class ProgramFileActivityGroup(
    IActivityTask<StorageActivityInput?, EndpointResult> activityForProgramFileDelete,
    IActivityTask<StorageActivityInput?, EndpointContentResult<string?>> activityForProgramFileRead,
    IActivityTask<StorageActivityInput?, EndpointContentResult<IReadOnlyCollection<StorageObject>>> activityForProgramFileList,
    IActivityTask<StorageActivityInput<string?>?, EndpointResult> activityForProgramFileSave,
    ILogger<ProgramFileActivityGroup>logger
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

            return await Task.FromResult(new EndpointResult(
                HttpStatusCode.InternalServerError,
                null,
                "Arguments were not valid."));
        }

        string? setKey = args[0];
        setKey.ThrowWhenNullOrWhiteSpace();

        string? bucketMetaKey = null;

        string? bucketKey = args.ElementAtOrDefault(2);
        string? content = args.ElementAtOrDefault(3);
        string? contentMimeType = args.ElementAtOrDefault(4);

        StorageActivityInput input = string.IsNullOrWhiteSpace(content) ?
                new StorageActivityInput(setKey, bucketMetaKey, bucketKey)
                :
                new StorageActivityInput<string?>(setKey, bucketMetaKey, bucketKey, content, contentMimeType);

        var activity = _activitySet.GetValueWithKey(activitySetKey);

        if (activity != null) return await activity.Invoke(input, cancellationToken);

        logger.LogError("The expected Activity, `{Name}`, is not here.", activitySetKey);

        return await Task.FromResult(new EndpointResult(
            HttpStatusCode.InternalServerError,
            null,
            "Activity was not found."));
    }

    private readonly Dictionary<string, Func<StorageActivityInput, CancellationToken, Task<EndpointResult>>> _activitySet = new()
    {
        [nameof(ProgramFileDeleteActivity)] = async (input, token) =>
        {
            EndpointResult result = await activityForProgramFileDelete.StartAsync(input, token);

            return result;
        },
        [nameof(ProgramFileReadActivity)] = async (input, token) =>
        {
            EndpointContentResult<string?> result = await activityForProgramFileRead.StartAsync(input, token);

            return result;
        },
        [nameof(ProgramFileListActivity)] = async (input, token) =>
        {
            EndpointContentResult<IReadOnlyCollection<StorageObject>> result = await activityForProgramFileList.StartAsync(input, token);

            return result;
        },
        [nameof(ProgramFileSaveActivity)] = async (input, token) =>
        {
            EndpointResult result = await activityForProgramFileSave.StartAsync(input as StorageActivityInput<string?>, token);

            return result;
        }
    };
}