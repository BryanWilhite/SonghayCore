using Microsoft.Extensions.Logging;

using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;

namespace SonghayCore.S3;

/// <summary>
/// Shared routines for <see cref="Amazon.S3"/>
/// </summary>
public static class S3Utility
{
    /// <summary>
    /// Returns an instance of <see cref="AmazonS3Client"/>
    /// or <c>null</c> when there is an error
    /// detailed by the <see cref="ILogger"/>
    /// </summary>
    /// <param name="credentialsProfileName">the AWS credentials profile name</param>
    /// <param name="uriRoot">the base URI of the desired S3 bucket</param>
    /// <param name="clientAppId">the value of <see cref="AmazonS3Config.ClientAppId"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static AmazonS3Client? GetAmazonS3Client(string? credentialsProfileName, string? uriRoot, string? clientAppId, ILogger logger)
    {
        AmazonS3Config config = new()
        {
            ServiceURL = uriRoot,
            ClientAppId = clientAppId
        };

        CredentialProfileStoreChain chain = new();

        if (!chain.TryGetAWSCredentials(credentialsProfileName, out AWSCredentials awsCredentials))
        {
            logger.LogError("The expected AWS profile, `{Name}`, was not found.", credentialsProfileName);

            return null;
        }

        AmazonS3Client s3Client = new (awsCredentials, config);

        return s3Client;
    }
}
