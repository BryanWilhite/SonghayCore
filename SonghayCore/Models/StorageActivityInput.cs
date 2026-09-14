namespace Songhay.Models;

/// <summary>
/// Defines the input for most storage operations.
/// </summary>
/// <param name="SetKey">
///     the <see cref="ProgramMetadata.RestApiMetadataSet"/> dictionary key
///     or the name of a directory under the root of a storage mirror
/// </param>
/// <param name="BucketMetaKey">
///     a <see cref="RestApiMetadata.ClaimsSet"/> dictionary key
///     pointing to the region of the S3 Bucket
/// </param>
/// <param name="BucketKeyOrPrefix">
///     represents an entire <c>S3Object.Key</c>
///     or its prefix (used for filtering S3 bucket listing)
/// </param>
public record StorageActivityInput(
    string SetKey,
    string? BucketMetaKey,
    string? BucketKeyOrPrefix
);

/// <summary>
/// Defines the input for most storage operations.
/// </summary>
/// <typeparam name="TContent">the type of the input content</typeparam>
/// <param name="SetKey">
///     the <see cref="ProgramMetadata.RestApiMetadataSet"/> dictionary key
///     or the name of a directory under the root of a storage mirror
/// </param>
/// <param name="BucketMetaKey">
///     a <see cref="RestApiMetadata.ClaimsSet"/> dictionary key
///     pointing to the region of the S3 Bucket
/// </param>
/// <param name="BucketKeyOrPrefix">
///     represents an entire <c>S3Object.Key</c>
///     or its prefix (used for filtering S3 bucket listing)
/// </param>
/// <param name="Content">represents what will be transformed into an S3 <c>PutObjectRequest.ContentBody</c></param>
/// <param name="ContentMimeType">represents an S3 <c>PutObjectRequest.ContentType</c></param>
public record StorageActivityInput<TContent>(
    string SetKey,
    string? BucketMetaKey,
    string? BucketKeyOrPrefix,
    TContent? Content,
    string? ContentMimeType
);
