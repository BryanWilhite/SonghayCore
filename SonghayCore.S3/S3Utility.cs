using Microsoft.Extensions.Logging;

using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;

namespace SonghayCore.S3;

public static class S3Utility
{
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
