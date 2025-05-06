using System.Text.Json.Nodes;

namespace Songhay.Tests.Extensions;

public class JsonNodeExtensionsTests(ITestOutputHelper helper)
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
        string? actual = JsonNode.Parse(input)?.AsObject().DisplayTopProperties(truncationLength);
        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": [] }", "my-property", "[]")]
    [InlineData("{ \"my-property\": [] }", "my-not-property", null)]
    public void GetPropertyJsonArrayOrNull_Test(string input, string propertyName, string? expectedOutput)
    {
        JsonArray? actual = JsonNode.Parse(input).GetPropertyJsonArrayOrNull(propertyName);
        Assert.Equal(expectedOutput, actual?.ToJsonString());
    }

    [Theory]
    [InlineData("{ \"my-property\": {} }", "my-property", "{}")]
    [InlineData("{ \"my-property\": {} }", "my-not-property", null)]
    public void GetPropertyJsonObjectOrNull_Test(string input, string propertyName, string? expectedOutput)
    {
        JsonObject? actual = JsonNode.Parse(input).GetPropertyJsonObjectOrNull(propertyName);
        Assert.Equal(expectedOutput, actual?.ToJsonString());
    }

    [Theory]
    [InlineData("{ \"my-property\": false }", "my-property", false)]
    [InlineData("{ \"my-property\": 2 }", "my-property", 2)]
    [InlineData("{ \"my-property\": \"two\" }", "my-property", "two")]
    [InlineData("{ \"my-property\": 42 }", "my-not-property", null)]
    [InlineData("{ \"my-property\": {} }", "my-property", null)]
    [InlineData("{ \"my-property\": [] }", "my-property", null)]
    [InlineData("{ \"my-property\": null }", "my-property", null)]
    public void GetPropertyJsonValueOrNull_Test(string input, string propertyName, object? expectedOutput)
    {
        JsonValue? actual = JsonNode.Parse(input).GetPropertyJsonValueOrNull(propertyName);

        switch (actual?.GetValueKind())
        {
            case JsonValueKind.String:
                Assert.Equal((string)expectedOutput!, actual.GetValue<string>());
                break;
            case JsonValueKind.Number:
                Assert.Equal((int)expectedOutput!, actual.GetValue<int>());
                break;
            case JsonValueKind.True:
            case JsonValueKind.False:
                Assert.Equal((bool)expectedOutput!, actual.GetValue<bool>());
                break;
            case null:
                Assert.Equal(expectedOutput, actual);
                break;
            default:
                Assert.Fail($"The expected {nameof(JsonValueKind)} is not here.");
                break;
        }
    }

    [Theory]
    [InlineData("{ \"my-property\":\"hello world!\" }", "my-property", "hello world!")]
    public void GetPropertyValue_String_Test(string input, string propertyName, string expectedOutput)
    {
        (string? value, bool success) actual = JsonNode.Parse(input).GetPropertyValue<string>(propertyName);
        Assert.True(actual.success);
        Assert.Equal(expectedOutput, actual.value);
    }

    [Theory]
    [InlineData("{ \"my-property\": false }", "my-property", false)]
    public void GetPropertyValue_Boolean_Test(string input, string propertyName, bool expectedOutput)
    {
        (bool? value, bool success) actual = JsonNode.Parse(input).GetPropertyValue<bool>(propertyName);
        Assert.True(actual.success);
        Assert.Equal(expectedOutput, actual.value);
    }

    [Theory] [InlineData("{ \"my-property\": 43.40 }", "my-property", 43.40)]
    public void GetPropertyValue_Decimal_Test(string input, string propertyName, decimal expectedOutput)
    {
        (decimal? value, bool success) actual = JsonNode.Parse(input).GetPropertyValue<decimal>(propertyName);
        Assert.True(actual.success);
        Assert.Equal(expectedOutput, actual.value);
    }

    [Theory]
    [InlineData("{ \"my-property\":\"hello world!\" }", "my-foo")]
    public void GetPropertyValue_Failure_Test(string input, string propertyName)
    {
        (string? value, bool success) actual = JsonNode.Parse(input).GetPropertyValue<string>(propertyName);
        Assert.False(actual.success);
        Assert.Equal(default, actual.value);
    }

    [Theory]
    [InlineData("{ \"my-property\": [] }", "my-property", JsonValueKind.Array)]
    [InlineData("{ \"my-property\": {} }", "my-property", JsonValueKind.Object)]
    [InlineData("{ \"my-property\": \"\" }", "my-property", JsonValueKind.String)]
    [InlineData("{ \"my-property\": null }", "my-property", JsonValueKind.Null)]
    [InlineData("{ \"my-property\": true }", "my-property", JsonValueKind.True)]
    [InlineData("{ \"my-property\": false }", "my-property", JsonValueKind.False)]
    [InlineData("{ \"my-property\": 42.0 }", "my-property", JsonValueKind.Number)]
    public void GetJsonValueKind_Test(string input, string propertyName, JsonValueKind expectedOutput)
    {
        JsonValueKind? actual = JsonNode.Parse(input)?[propertyName].GetJsonValueKind();
        Assert.NotNull(actual);
        Assert.Equal(expectedOutput, actual.Value);
    }

    [Theory]
    [InlineData("{ \"my-property\": [] }", "my-property", true)]
    [InlineData("{ \"my-property\": [] }", "my-not-property", false)]
    [InlineData("{}", "my-not-property", false)]
    public void HasProperty_Test(string input, string propertyName, bool expectedOutput)
    {
        bool actual = JsonNode.Parse(input).HasProperty(propertyName);
        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("{\"a\":1, \"b\":2, \"c\":3}", true, "a","b","c")]
    [InlineData("[1,2,3]", false, "")]
    public void IsExpectedObject_Test(string input, bool expectedOutput, params string[] properties)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(IsExpectedObject_Test));
        bool actual = JsonNode.Parse(input).IsExpectedObject(logger, properties);
        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("""
                [
                    {
                        "one": 1.0
                    },
                    {
                        "three": 3.0,
                        "four": 4.0
                    }
                ]
                """)]
    public void RemoveFromParent_JsonArray_First_JsonObject_Test(string input)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(ToJsonArray_Success_Test));
        JsonArray? parent = JsonNode.Parse(input)?.AsArray();
        JsonObject? actual = parent?.First()?.AsObject();

        actual.RemoveFromParent(logger);

        logger.LogDebug("JSON removed: {Json}", actual?.ToJsonString());
        logger.LogDebug("JSON parent: {Json}", parent?.ToJsonString());

        Assert.Null(actual!.Parent);
        Assert.Single(parent!);
    }

    [Theory]
    [InlineData("""
                [
                    1,
                    2,
                    3
                ]
                """)]
    public void RemoveFromParent_JsonArray_Third_Number_Test(string input)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(ToJsonArray_Success_Test));
        JsonArray? parent = JsonNode.Parse(input)?.AsArray();
        JsonValue? actual = parent?.Last()?.AsValue();

        actual.RemoveFromParent(logger);

        logger.LogDebug("JSON removed: {Json}", actual?.ToJsonString());
        logger.LogDebug("JSON parent: {Json}", parent?.ToJsonString());

        Assert.Null(actual!.Parent);
        Assert.Equal(2, parent!.Count);
    }

    [Theory]
    [InlineData("""
                {
                    "my-property": {
                        "one": 1.0
                    },
                    "my-other-property": {
                        "two": 2.0
                    }
                }
                """, "my-property")]
    public void RemoveFromParent_JsonObject_Test(string input, string propertyName)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(ToJsonArray_Success_Test));
        JsonObject? parent = JsonNode.Parse(input)?.AsObject();
        JsonObject? actual = parent.GetPropertyJsonObjectOrNull(propertyName);

        actual.RemoveFromParent(propertyName, logger);

        logger.LogDebug("JSON removed: {Json}", actual?.ToJsonString());
        logger.LogDebug("JSON parent: {Json}", parent?.ToJsonString());

        Assert.Null(actual!.Parent);
        Assert.Null(parent.GetPropertyJsonObjectOrNull(propertyName));
    }

    [Theory]
    [InlineData("""
                {
                    "my-property": {
                        "one": 1.0
                    },
                    "my-other-property": {
                        "two": 2.0
                    }
                }
                """, "my-property")]
    [InlineData("""
                [
                    {
                        "one": 1.0
                    },
                    {
                        "one": 1.0,
                        "two": 2.0
                    }
                ]
                """, "one")]
    public void RemoveProperty_Test(string input, string propertyName)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(ToJsonArray_Success_Test));
        JsonNode? actual = JsonNode.Parse(input);

        actual.RemoveProperty(propertyName, logger);
        logger.LogDebug("JSON: {Json}", actual?.ToJsonString());

        switch (actual?.GetValueKind())
        {
            case JsonValueKind.Array:
                Assert.All(actual.AsArray(),
                    n => Assert.False(n.HasProperty(propertyName)));
                break;
            case JsonValueKind.Object:
                Assert.False(actual.HasProperty(propertyName));
                break;
            default:
                Assert.Fail("Kind is not supported.");
                break;
        }
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("[1,2,3]")]
    public void ToJsonArray_Success_Test(string input)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(ToJsonArray_Success_Test));
        JsonArray? actual = JsonNode.Parse(input).ToJsonArray(logger);
        Assert.NotNull(actual);
    }

    [Theory]
    [InlineData("{}")]
    public void ToJsonArray_Failure_Test(string input)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(ToJsonArray_Failure_Test));
        JsonArray? actual = JsonNode.Parse(input).ToJsonArray(logger);
        Assert.Null(actual);
    }

    [Theory]
    [InlineData("{ \"my-property\": 42 }", "my-property", "my-other-property")]
    [InlineData("{ \"my-property\": 42 }", "my-non-property", "my-other-property")]
    [InlineData("{ \"my-property\": null }", "my-non-property", "my-other-property")]
    public void WithPropertiesRenamed_Test(string input, string oldName, string newName)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(IsExpectedObject_Test));
        JsonObject? actual = JsonNode.Parse(input)?.AsObject().WithPropertiesRenamed(logger, (oldName, newName));
        Assert.False(actual.HasProperty(oldName));
    }

    private readonly XUnitLoggerProvider _loggerProvider = new(helper);
}
