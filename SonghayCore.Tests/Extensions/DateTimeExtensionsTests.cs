using System.Globalization;
using System.Text.Json.Nodes;

namespace Songhay.Tests.Extensions;

public class DateTimeExtensionsTests(ITestOutputHelper testOutputHelper)
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

    /// <remarks>
    /// This impoverished test verifies that I am not the inventor of Noda Time.
    /// The official docs clearly state that <see cref="DateTime.Parse(string)"/>
    /// will default to the current culture.
    /// (See https://learn.microsoft.com/en-us/dotnet/api/system.datetime.parse?view=net-10.0)
    /// </remarks>
    [Theory]
    [InlineData("2026-06-03T19:00:00Z")]
    public void ShouldConvertIso8601UtcStringToLocal(string input)
    {
        DateTime actual = DateTime.Parse(input);
        DateTime expected = actual.ToUniversalTime();

        testOutputHelper.WriteLine($"input: `{input}`");
        testOutputHelper.WriteLine($"current-culture offset: {DateTimeOffset.Now.Offset.Hours}");
        testOutputHelper.WriteLine($"actual hour: {actual.Hour}");
        testOutputHelper.WriteLine($"actual kind: {actual.Kind}");
        testOutputHelper.WriteLine($"expected hour: {expected.Hour}");
        testOutputHelper.WriteLine($"expected kind: {expected.Kind}");

        Assert.NotEqual(expected, actual);
    }

    /// <remarks>
    /// This test shows how to parse an ISO 8601 UTC string
    /// under <see cref="CultureInfo.InvariantCulture"/>
    /// while preserving UTC time.
    /// This test also shows that calling <see cref="DateTime.ToUniversalTime"/>
    /// on a value that is already <see cref="DateTimeKind.Utc"/> will not erroneously convert it again.
    /// </remarks>
    [Theory]
    [InlineData("2026-06-03T19:00:00Z")]
    public void ShouldConvertIso8601UtcString(string input)
    {
        DateTime actual = DateTime.Parse(input, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        DateTime expected = actual.ToUniversalTime();

        testOutputHelper.WriteLine($"input: `{input}`");
        testOutputHelper.WriteLine($"current-culture offset: {DateTimeOffset.Now.Offset.Hours}");
        testOutputHelper.WriteLine($"actual hour: {actual.Hour}");
        testOutputHelper.WriteLine($"actual kind: {actual.Kind}");
        testOutputHelper.WriteLine($"expected hour: {expected.Hour}");
        testOutputHelper.WriteLine($"expected kind: {expected.Kind}");

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToIso8601String_Test()
    {
        string dateString = DateTime.Now.ToIso8601String();
        testOutputHelper.WriteLine(dateString);

        DateTime expected = DateTime.Parse(dateString).ToUniversalTime();

        string json = $"{{ \"one\": {{ \"my-date\": \"{dateString}\" }} }}";
        using JsonDocument jDoc = JsonDocument.Parse(json);
        DateTime actual = jDoc.RootElement.GetProperty("one").GetProperty("my-date").GetDateTime();

        Assert.Equal(expected, actual);

        JsonNode jO = JsonNode.Parse(json).ToReferenceTypeValueOrThrow();
        actual = jO["one"]?["my-date"]?.GetValue<DateTime>() ?? DateTime.Now;

        Assert.Equal(expected, actual);
    }
}
