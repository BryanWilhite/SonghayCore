using System.Text.Json.Nodes;

namespace Songhay.Tests.Extensions;

public class JsonObjectExtensionsTests(ITestOutputHelper helper)
{
    [Theory]
    [InlineData("""
                {
                    "my-property": {
                        "one": 1,
                        "sure": true
                    }
                }
                """, 16, "my-property: {\"one\":1,\"sure\":…\n")]
    [InlineData("""
                {
                    "my-property": {
                        "one": 1.0,
                        "sure": true
                    }
                }
                """, 16, "my-property: {\"one\":1.0,\"sure…\n")]
    [InlineData("""
                {
                    "my-property": {
                        "one": null,
                        "others": ["y","n","u"]
                    }
                }
                """, 24, "my-property: {\"one\":null,\"others\":[\"y…\n")]
    public void DisplayTopProperties_Test(string input, int truncationLength, string expectedOutput)
    {
        //act:
        string? actual = JsonNode.Parse(input)?.AsObject().DisplayTopProperties(truncationLength);

        //assert:
        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("""
                {
                    "myArray": [
                        2,
                        4
                    ]
                }
                """, "myArray", 42)]
    public void ShouldAddIntItemToArray(string input, string? arrayPropertyName, int? item)
    {
        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldAddIntItemToArray));
        JsonObject? jsonObject = JsonNode.Parse(input)?.AsObject();

        //act:
        jsonObject.AddItemToArray(arrayPropertyName, item, logger);
        int?[] actual =
        [
            .. jsonObject
                .GetPropertyJsonArrayOrNull(arrayPropertyName!)
                .ToJsonArray(logger).ToReferenceTypeValueOrThrow()
                .OfType<JsonNode>()
                .Select(n => n.GetValue<int?>())
        ];

        //assert:
        Assert.Contains(item, actual);
    }

    [Theory]
    [InlineData("""
                {
                    "myProperty": true
                }
                """, "myProperty")]
    public void ShouldGetPropertyJsonNodeOrNullForBool(string input, string? arrayPropertyName)
    {
        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ShouldAddIntItemToArray));
        JsonObject? jsonObject = JsonNode.Parse(input)?.AsObject();

        //act:
        var actual = jsonObject
            .GetPropertyJsonNodeOrNull(arrayPropertyName, logger)
            .ToReferenceTypeValueOrThrow()
            .GetValue<bool>();

        //assert:
        Assert.True(actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": 42 }", "my-property", "my-other-property")]
    [InlineData("{ \"my-property\": 42 }", "my-non-property", "my-other-property")]
    [InlineData("{ \"my-property\": null }", "my-non-property", "my-other-property")]
    public void WithPropertiesRenamed_Test(string input, string oldName, string newName)
    {
        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(WithPropertiesRenamed_Test));

        //act:
        JsonObject? actual = JsonNode.Parse(input)?.AsObject().WithPropertiesRenamed(logger, (oldName, newName));

        //assert:
        Assert.False(actual.HasProperty(oldName));
    }

    private readonly XUnitLoggerProvider _loggerProvider = new(helper);
}
