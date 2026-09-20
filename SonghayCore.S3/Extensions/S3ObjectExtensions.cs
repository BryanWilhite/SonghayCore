namespace Songhay.S3.Extensions;

/// <summary>
/// Extensions of <see cref="S3Object"/>
/// </summary>
public static class S3ObjectExtensions
{
    /// <summary>
    /// Transforms <see cref="S3Object"/>
    /// into <see cref="StorageObject"/>.
    /// </summary>
    /// <param name="s3Object">the <see cref="S3Object"/></param>
    public static StorageObject? ToStorageObject(this S3Object? s3Object)
    {
        if (s3Object == null) return null;

        return new StorageObject(
                s3Object.BucketName,
                s3Object.ETag,
                s3Object.Key,
                s3Object.LastModified.GetValueOrDefault(),
                s3Object.Size.GetValueOrDefault(),
                null,
                s3Object.Key.Split('/').Last()
            );
    }
}
