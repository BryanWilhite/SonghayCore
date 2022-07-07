using Songhay.Extensions;
using Songhay.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions;

public class RestApiMetadataExtensionsTest
{
    public RestApiMetadataExtensionsTest(ITestOutputHelper helper)
    {
        this._testOutputHelper = helper;
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

        this._testOutputHelper.WriteLine(meta.ToUri(key, "Washington", "Redmond", "Today").OriginalString);
    }

    readonly ITestOutputHelper _testOutputHelper;
}