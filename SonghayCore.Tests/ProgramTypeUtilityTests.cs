using System.Text.Json.Nodes;

namespace Songhay.Tests;

public class ProgramTypeUtilityTests
{

    [Fact]
    public void ConvertDateTimeToUtc_Test()
    {
        var date = DateTime.Now;
        var dateString = ProgramTypeUtility.ConvertDateTimeToUtc(date);

        var expected = DateTime.Parse(dateString).ToUniversalTime();

        var json = $"{{ \"one\": {{ \"my-date\": \"{dateString}\" }} }}";
        var jDoc = JsonDocument.Parse(json);
        var actual = jDoc.RootElement.GetProperty("one").GetProperty("my-date").GetDateTime();

        Assert.Equal(expected, actual);

        var jO = JsonNode.Parse(json).ToReferenceTypeValueOrThrow();
        actual = jO["one"]?["my-date"]?.GetValue<DateTime>() ?? DateTime.Now;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("Warning", SourceLevels.All, SourceLevels.Warning)]
    [InlineData("Warning,Critical,Error", SourceLevels.All, SourceLevels.All)]
    public void ShouldParseEnum(string input, SourceLevels defaultEnum, SourceLevels expectedEnum)
    {
        var enumValue = ProgramTypeUtility.ParseEnum(input, defaultEnum);
        Assert.Equal(expectedEnum, enumValue);
    }
}