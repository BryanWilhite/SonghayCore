namespace Songhay.Tests;

public class ProgramTypeUtilityTests
{
    [Theory]
    [InlineData("2026-06-03T19:00:00")]
    public void ParseToUtcDateTimeFromLocalTime_Test(string input)
    {
        //arrange:
        DateTime local = ProgramTypeUtility.ParseToLocalTime(input).ToValueOrThrow();
        DateTime utc = ProgramTypeUtility.ParseToUtcDateTimeFromLocalTime(input).ToValueOrThrow();

        //act:
        TimeSpan actual = local - utc;

        //assert:
        Assert.True(Math.Abs(actual.TotalHours) > 0);
        Assert.Equal(TimeZoneInfo.Local.GetUtcOffset(local), actual);
    }

    [Theory]
    [InlineData("Warning", SourceLevels.All, SourceLevels.Warning)]
    [InlineData("Warning,Critical,Error", SourceLevels.All, SourceLevels.All)]
    public void ShouldParseEnum(string input, SourceLevels defaultEnum, SourceLevels expectedEnum)
    {
        SourceLevels enumValue = ProgramTypeUtility.ParseEnum(input, defaultEnum);
        Assert.Equal(expectedEnum, enumValue);
    }
}