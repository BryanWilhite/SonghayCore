using Microsoft.Extensions.Logging;

using Songhay.Extensions;
using Songhay.Models;

namespace SonghayCore.S3.Extensions;

/// <summary>
/// Extensions of <see cref="RestApiMetadata"/>
/// </summary>
public static class RestApiMetadataExtensions
{
    /// <summary>
    /// Returns a tuple, decomposing the specified <see cref="RestApiMetadata"/>
    /// into:
    ///
    /// - AWS credentials profile name (on Linux in the <c>~/.aws/credentials</c> file)
    /// - the S3 bucket name
    /// - the S3 bucket region
    /// - the base URI of the bucket
    ///
    /// </summary>
    /// <param name="meta">the <see cref="RestApiMetadata"/></param>
    /// <param name="bucketMetaKey">the <see cref="RestApiMetadata.ClaimsSet"/> dictionary key</param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    /// <remarks>
    /// The JSON shaped like <see cref="RestApiMetadata"/> can look like this:
    /// 
    /// <code>
    /// "Wasabi": {
    ///     "ClaimsSet": {
    ///         "aws-credentials-profile-name": "???",
    ///         "bucket-region-suffix": "-region",
    ///         "public-key": "???",
    ///         "private-key": "??",
    ///         "bucket-location-template": "https://s3.{Region}.wasabisys.com/",
    ///         "bucket-location-template-placeholder": "{Region}",
    ///         "my-bucket-region": "us-central-1"
    ///     }
    /// }
    /// </code>
    ///
    /// …where the name of the bucket, <c>my-bucket</c>, is ‘embedded’
    /// in the <c>my-bucket-region</c> key-value pair.
    /// </remarks>
    public static (string? credentialsProfileName, string? bucketName, string? region, string? uriRoot) ToS3Tuple(this RestApiMetadata? meta, string? bucketMetaKey, ILogger logger)
    {
        const string regionSuffix = "-region";
        const string regionPlaceHolder = "{Region}";

        Dictionary<string, string>? claimsSet = meta?.ClaimsSet;

        if (claimsSet == null)
        {
            logger.LogError("The expected input, {Name}, is not here.", nameof(claimsSet));

            return default;
        }
        
        if (string.IsNullOrWhiteSpace(bucketMetaKey))
        {
            logger.LogError("The expected input, {Name}, is not here.", nameof(bucketMetaKey));

            return default;
        }

        string bucketName = bucketMetaKey.Replace(regionSuffix, string.Empty);
        string? credentialsProfileName = claimsSet.TryGetValueWithKey("aws-credentials-profile-name");
        string? region = claimsSet.TryGetValueWithKey(bucketMetaKey);
        string? uriRoot = claimsSet.TryGetValueWithKey("bucket-location-template");

        if (!string.IsNullOrWhiteSpace(uriRoot))
        {
            uriRoot = uriRoot.Replace(regionPlaceHolder, region);
        }

        return (credentialsProfileName, bucketName, region, uriRoot);
    }
}
