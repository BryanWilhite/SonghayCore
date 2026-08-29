namespace Songhay.S3.Abstractions;

/// <summary>
/// Defines the domain-specific grouping of <c>AmazonS3*Activity</c> activities
/// and maps the input(s) of these Activities with its respective Activity.
/// </summary>
public interface IAmazonS3ActivityGroup
{
    /// <summary>
    /// The input invocations of <c>AmazonS3*Activity</c> activities.
    /// </summary>
    /// <param name="activitySetKey">the name of the activity type</param>
    /// <param name="setKey">the name of the key for the <see cref="ProgramMetadata.RestApiMetadataSet"/></param>
    /// <param name="bucketMetaKey">a key in the <see cref="RestApiMetadata.ClaimsSet"/></param>
    /// <param name="bucketKey"><see cref="S3Object.Key"/></param>
    /// <param name="content"><see cref="PutObjectRequest.ContentBody"/> or equivalent</param>
    /// <param name="contentMimeType"><see cref="PutObjectRequest.ContentType"/> or equivalent</param>
    /// <remarks>
    /// The arguments of this method represent a superset
    /// of all the parameter-tuples used
    /// for each <c>AmazonS3*Activity</c> activity.
    /// </remarks>
    Task<string?> InvokeActivityAsync(string activitySetKey, string setKey, string bucketMetaKey, string? bucketKey, string? content, string? contentMimeType);
}
