namespace SonghayCore.S3.Abstractions;

public interface IActivityGroup
{
    Task<string?> InvokeActivityAsync(string activitySetKey, string setKey, string bucketMetaKey, string? bucketKey, string? content, string? contentMimeType);
}
