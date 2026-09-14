namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="StorageActivityInput"/>
/// </summary>
public static class StorageActivityInputExtensions
{
    /// <summary>
    /// Transforms <see cref="StorageActivityInput"/>
    /// into <see cref="FileInfo"/>
    /// </summary>
    /// <param name="input">the <see cref="StorageActivityInput"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static FileInfo? ToFileInfo(this StorageActivityInput? input, ILogger logger)
    {
        if (input == null)
        {
            logger.LogErrorForMissingData<StorageActivityInput>();

            return null;
        }

        if (!Directory.Exists(input.SetKey))
        {
            logger.LogError("The expected root directory is not here.");

            return null;
        }

        DirectoryInfo directoryInfo = new(input.SetKey);

        string? path = directoryInfo.ToCombinedPath(input.BucketKeyOrPrefix);

        if (string.IsNullOrWhiteSpace(path))
        {
            logger.LogError("The expected path is not here.");

            return null;
        }

        return new FileInfo(path);
    }

    /// <summary>
    /// Transforms <see cref="StorageActivityInput"/>
    /// into a collection of <see cref="StorageObject"/>
    /// </summary>
    /// <param name="input">the <see cref="StorageActivityInput"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static IReadOnlyCollection<StorageObject> ToStorageObjects(this StorageActivityInput? input, ILogger logger)
    {
        if (input == null)
        {
            logger.LogErrorForMissingData<StorageActivityInput>();

            return [];
        }

        if (!Directory.Exists(input.SetKey))
        {
            logger.LogError("The expected root directory is not here.");

            return [];
        }

        DirectoryInfo directoryInfo = new(input.SetKey);

        string? filter = $"{input.BucketKeyOrPrefix}*";

        FileSystemInfo[] filtered = directoryInfo.GetFileSystemInfos(filter);

        return
            [..
                filtered
                    .SelectMany(fi => fi switch
                    {
                        FileInfo file => [file.ToStorageObject()],
                        DirectoryInfo dir => dir.EnumerateFiles("*", SearchOption.AllDirectories).Select(file => file.ToStorageObject()),
                        _ => []
                    })
                    .OfType<StorageObject>()
            ];
    }
}
