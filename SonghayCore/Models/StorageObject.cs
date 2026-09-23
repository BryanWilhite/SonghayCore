namespace Songhay.Models;

/// <summary>
/// Defines an “intersection type” for the <c>S3Object</c> defacto standard
/// and <see cref="FileInfo"/>
/// </summary>
/// <param name="BucketName">maps to <see cref="FileInfo.DirectoryName"/></param>
/// <param name="ETag">represents <c>S3Object.ETag</c></param>
/// <param name="Key">represents <c>S3Object.Key</c></param>
/// <param name="LastWriteTimeUtc">maps to <c>S3Object.LastModified</c></param>
/// <param name="Size">maps to <see cref="FileInfo.Length"/></param>
/// <param name="Extension">represents <see cref="FileSystemInfo.Extension"/></param>
/// <param name="Name">represents <see cref="FileInfo.Name"/></param>
public record StorageObject(
    string BucketName,
    string? ETag,
    string Key,
    DateTime LastWriteTimeUtc,
    long Size,
    string? Extension,
    string Name
);
