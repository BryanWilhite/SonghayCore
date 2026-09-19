namespace Songhay.Activities;

/// <summary>
/// Reads the directory
/// from the local file system,
/// impersonating the input and output
/// of <c>AmazonS3ListBucketObjectsWithPaginationActivity</c>
/// </summary>
/// <param name="logger">the <see cref="ILogger"/></param>
public class ProgramFileListActivity(ILogger<ProgramFileListActivity> logger) : IActivityTask<StorageActivityInput?, EndpointContentResult<IReadOnlyCollection<StorageObject>>>
{
    /// <inheritdoc/>
    public async Task<EndpointContentResult<IReadOnlyCollection<StorageObject>>> StartAsync(StorageActivityInput? input, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<StorageObject> storageObjects = await Task.Run(() => input.ToStorageObjects(logger), cancellationToken);
        return
            storageObjects.Count > 0 ?
                new EndpointContentResult<IReadOnlyCollection<StorageObject>>(
                    HttpStatusCode.OK,
                    "[local]",
                    $"{storageObjects.Count} objects found.",
                    storageObjects
                )
                :
                new EndpointContentResult<IReadOnlyCollection<StorageObject>>(
                    HttpStatusCode.NotFound,
                    "[local]",
                    "The expected list of objects is not here.",
                    storageObjects
                );
    }
}
