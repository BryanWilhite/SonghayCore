using System.Text.RegularExpressions;
using Songhay.Globalization;
using Xunit;

namespace Songhay.Tests.Globalization;

public class TextTests
{
    [Fact]
    public void ToTitleCase_Test()
    {
        var input = "over the dale and between the two trees";
        var expected = "Over the Dale and between the Two Trees";
        var actual = TextInfoUtility.ToTitleCase(input);
        Assert.Equal(expected, actual);

        input = "this, yes, but also that!";
        expected = "This, Yes, but also That!";
        actual = TextInfoUtility.ToTitleCase(input);
        Assert.Equal(expected, actual);

        input = "golfer versus boxer";
        expected = "Golfer versus Boxer";
        actual = TextInfoUtility.ToTitleCase(input);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Regex_Test()
    {
        var s = "she’s here!";
        s = Regex.Replace(s, @"(\w)'(\w)", "$1’$2");
        Assert.Contains("’", s);
    }
}