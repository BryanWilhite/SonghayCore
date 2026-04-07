namespace Songhay.Tests;

public class JsonElementUtilityTests(ITestOutputHelper helper)
{
    [Theory]
    [InlineData(null, JsonValueKind.Null)]
    [InlineData("{{", JsonValueKind.Null)]
    [InlineData("[]", JsonValueKind.Array)]
    [InlineData("42", JsonValueKind.Number)]
    public void ParseJson_Test(string? json, JsonValueKind expected)
    {
        //arrange:
        ILogger logger = _loggerProvider.CreateLogger(nameof(ParseJson_Test));

        // act:
        JsonElement actual = JsonElementUtility.ParseJson(json, logger);

        // assert:
        Assert.Equal(expected, actual.ValueKind);
    }

    readonly XUnitLoggerProvider _loggerProvider = new(helper);
}
