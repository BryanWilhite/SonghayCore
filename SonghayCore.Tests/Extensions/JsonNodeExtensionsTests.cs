using System.Text.Json.Nodes;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.Extensions.Logging;

namespace Songhay.Tests.Extensions;

public class JsonNodeExtensionsTests(ITestOutputHelper helper)
{
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
    [InlineData("{\"a\":1, \"b\":2, \"c\":3}", true, "a","b","c")]
    [InlineData("[1,2,3]", false, "")]
    public void IsExpectedObject_Test(string input, bool expectedOutput, params string[] properties)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(IsExpectedObject_Test));
        bool actual = JsonNode.Parse(input).IsExpectedObject(logger, properties);
        Assert.Equal(expectedOutput, actual);
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("[1,2,3]")]
    public void ToJsonArray_Success_Test(string input)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(IsExpectedObject_Test));
        JsonArray? actual = JsonNode.Parse(input).ToJsonArray(logger);
        Assert.NotNull(actual);
    }

    [Theory]
    [InlineData("{}")]
    public void ToJsonArray_Failure_Test(string input)
    {
        ILogger logger = _loggerProvider.CreateLogger(nameof(IsExpectedObject_Test));
        JsonArray? actual = JsonNode.Parse(input).ToJsonArray(logger);
        Assert.Null(actual);
    }

    readonly XUnitLoggerProvider _loggerProvider = new(helper);
}
