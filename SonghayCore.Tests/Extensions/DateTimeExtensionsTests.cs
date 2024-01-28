using System.Text.Json.Nodes;

namespace Songhay.Tests.Extensions;

public class DateTimeExtensionsTests
{
    [Theory]
    [InlineData("Monday, January 15, 2024", DayOfWeek.Wednesday, "Wednesday, January 17, 2024")]
    [InlineData("Monday, January 15, 2024", DayOfWeek.Sunday, "Sunday, January 21, 2024")]
    public void GetNextWeekday_Test(string startText, DayOfWeek day, string expectedText)
    {
        var start = DateTime.Parse(startText);
        var expected = DateTime.Parse(expectedText);

        var actual = start.GetNextWeekday(day);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToIso8601String_Test()
    {
        var dateString = DateTime.Now.ToIso8601String();

        var expected = DateTime.Parse(dateString).ToUniversalTime();

        var json = $"{{ \"one\": {{ \"my-date\": \"{dateString}\" }} }}";
        var jDoc = JsonDocument.Parse(json);
        var actual = jDoc.RootElement.GetProperty("one").GetProperty("my-date").GetDateTime();

        Assert.Equal(expected, actual);

        var jO = JsonNode.Parse(json).ToReferenceTypeValueOrThrow();
        actual = jO["one"]?["my-date"]?.GetValue<DateTime>() ?? DateTime.Now;

        Assert.Equal(expected, actual);
    }
}
