using Songhay.Models;

namespace Songhay.Tests.Extensions;

public class RestApiMetadataExtensionsTests(ITestOutputHelper helper)
{
    public static readonly IEnumerable<object[]> MetaData =
    [
        [
            "weather-template",
                new RestApiMetadata
                {
                    ApiBase = new Uri("https://my.api.me", UriKind.Absolute),
                    UriTemplates = new Dictionary<string, string>
                    {
                        { "weather-template", "weather/{state}/{city}?forecast={day}" },
                    }
                },
                new[] { "Washington", "Redmond", "Today" },
                "https://my.api.me/weather/Washington/Redmond?forecast=Today"
        ]
    ];

    [Theory]
    [MemberData(nameof(MetaData))]
    public void ToUri_Test(string key, RestApiMetadata meta, string[] input, string expected)
    {
        Uri actual = meta.ToUri(key, input).ToReferenceTypeValueOrThrow();
        helper.WriteLine($"{nameof(actual)}: {actual.OriginalString}");
        Assert.Equal(expected, actual.OriginalString);
    }

    [Theory]
    [ProjectFileData(
        typeof(RestApiMetadataExtensionsTests),
        "../../../json/studio-metadata.json")]
    public void ToAzureActiveDirectoryAccessTokenData_Test(FileInfo metaInfo)
    {
        var json = File.ReadAllText(metaInfo.FullName);
        var meta = AzureActiveDirectoryUtility.GetActiveDirectoryAccessMetadata(json);

        var actual = meta.ToAzureActiveDirectoryAccessTokenData();
        Assert.NotEmpty(actual);
    }

    [Theory]
    [ProjectFileData(
        typeof(RestApiMetadataExtensionsTests),
        new object[] {
            "https://login.microsoftonline.com/active-directory-registration-app-directory-id/oauth2/v2.0/token"
        },
        "../../../json/studio-metadata.json")]
    public void ToAzureActiveDirectoryAccessTokenUri_Test(string expected, FileInfo metaInfo)
    {
        var json = File.ReadAllText(metaInfo.FullName);
        var meta = AzureActiveDirectoryUtility.GetActiveDirectoryAccessMetadata(json);

        var actual = meta.ToAzureActiveDirectoryAccessTokenUri();
        helper.WriteLine($"{nameof(actual)}: {actual.OriginalString}");
        Assert.Equal(expected, actual.OriginalString);
    }

    [Theory]
    [ProjectFileData(
        typeof(RestApiMetadataExtensionsTests),
        new object[] {
            "secretNameForMySecret",
            "https://your-secrets.vault.azure.net:443/secrets/my-secret?api-version=2016-10-01"
        },
        "../../../json/studio-metadata.json")]
    public void ToAzureKeyVaultSecretUri_Test(string secretNameKey, string expected, FileInfo metaInfo)
    {
        var json = File.ReadAllText(metaInfo.FullName);
        var meta = AzureActiveDirectoryUtility.GetAzureKeyVaultMetadata(json);

        var actual = meta.ToAzureKeyVaultSecretUri(secretNameKey);
        helper.WriteLine($"{nameof(actual)}: {actual.OriginalString}");
        Assert.Equal(expected, actual.OriginalString);
    }
}
