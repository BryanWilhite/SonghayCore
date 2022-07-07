using Songhay.Exceptions;
using Songhay.Extensions;
using Xunit;

namespace Songhay.Tests;

public partial class StringExtensionsTests
{
    [Fact]
    public void ShouldSplit()
    {
        var split = "\"123\",\"456\",\"789\"".CsvSplit();
        var test = Validate(split, new[] { "123", "456", "789" });
        Assert.True(test, "The Validate method failed.");
    }

    [Fact]
    public void ShouldSplitWithBackSlash()
    {
        var split = "\"12\\\"3\",\"456\",\"789\"".CsvSplit();
        var test = Validate(split, new[] { "12\"3", "456", "789" });
        Assert.True(test, "The Validate method failed.");
    }

    [Fact]
    public void ShouldSplitWithComma()
    {
        var split = "\"aaa,bbb\",\"ccc,ddd\",ghi".CsvSplit();
        var test = Validate(split, new[] { "aaa,bbb", "ccc,ddd", "ghi" });
        Assert.True(test, "The Validate method failed.");
    }

    [Fact]
    public void ShouldSplitWithDoubleBackslash()
    {
        var split = "\"a\\\\aa\",,bbb,".CsvSplit();
        var test = Validate(split, new[] { "a\\aa", "", "bbb", "" });
        Assert.True(test, "The Validate method failed.");
    }

    [Theory]
    [InlineData("aaa,,bbb", new[] { "aaa", "", "bbb" })]
    [InlineData("\"a\\aa\",,bbb,", new[] { "aaa", "", "bbb", "" })]
    public void ShouldSplitWithEmpty(string input, string[] expected)
    {
        var split = input.CsvSplit();
        var test = Validate(split, expected);
        Assert.True(test, "The Validate method failed.");
    }

    [Fact]
    public void ShouldSplitWithEmptyTrailing()
    {
        var split = "aaa,,bbb,".CsvSplit();
        var test = Validate(split, new[] { "aaa", "", "bbb", "" });
        Assert.True(test, "The Validate method failed.");
    }

    [Theory]
    [InlineData("\"aaabbb\"bbb,ccc,ddd")]
    [InlineData("\"aaabbb\",ccc,\"ddd")]
    [InlineData("aaa,ccc,\"ddd\\")]
    public void ShouldThrowCsvParseException(string input)
    {
        Assert.Throws<CsvParseException>(() => input.CsvSplit());
    }

    static bool Validate(string[] results, string[] expectedResults)
    {
        if (results.Length != expectedResults.Length)
        {
            return false;
        }
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] != expectedResults[i])
            {
                return false;
            }
        }
        return true;
    }
}