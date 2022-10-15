using Songhay.Models;

namespace Songhay.Tests.Extensions;

public class RestApiMetadataExtensionsTest
{
    public RestApiMetadataExtensionsTest(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
    }

    [Fact]
    public void ShouldConvertToUri()
    {
        var key = "weather-template";
        var meta = new RestApiMetadata
        {
            ApiBase = new Uri("https://my.api.me", UriKind.Absolute),
            UriTemplates = new Dictionary<string, string>
            {
                { key, "weather/{state}/{city}?forecast={day}" },
            }
        };

        _testOutputHelper.WriteLine(
            meta.ToUri(key, "Washington", "Redmond", "Today")
                .ToReferenceTypeValueOrThrow()
                .OriginalString);
    }

    readonly ITestOutputHelper _testOutputHelper;
}
