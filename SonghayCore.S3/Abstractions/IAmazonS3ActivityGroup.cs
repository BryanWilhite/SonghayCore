namespace SonghayCore.S3.Abstractions;

public interface IAmazonS3ActivityGroup
{
    Task<string?> InvokeActivityAsync(string activitySetKey, string setKey, string bucketMetaKey, string? bucketKey, string? content, string? contentMimeType);
}
