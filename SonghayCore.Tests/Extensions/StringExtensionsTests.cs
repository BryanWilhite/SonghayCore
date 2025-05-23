﻿namespace Songhay.Tests.Extensions;

public partial class StringExtensionsTests(ITestOutputHelper helper)
{
    [Theory]
    [InlineData(@"{ ""my"": ""json"" }")]
    public void EscapeInterpolation_Test(string input)
    {
        Assert.Throws<FormatException>(() => helper.WriteLine(input, 0));
        helper.WriteLine(input.EscapeInterpolation(), 0);
    }

    [Theory]
    [InlineData("FromCamelCaseToEnumerable", 5)]
    public void FromCamelCaseToEnumerable_Test(string input, int expectedCount)
    {
        var data = input.FromCamelCaseToEnumerable();
        Assert.Equal(expectedCount, data.Count());
    }

    [Theory]
    [InlineData("one_two_three", "oneTwoThree")]
    public void FromSnakeToCaps_Test(string input, string expected)
    {
        var actual = input.FromSnakeToCaps();
        helper.WriteLine($"{nameof(input)}: {input}");
        helper.WriteLine($"{nameof(expected)}: {expected}");
        helper.WriteLine($"{nameof(actual)}: {actual}");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("", "nil", "nil")]
    [InlineData("one", "nil", @"""one""")]
    public void InDoubleQuotesOrDefault_Test(string input, string defaultValue, string expected)
    {
        var actual = input.InDoubleQuotesOrDefault(defaultValue);
        helper.WriteLine($"{nameof(input)}: {input}");
        helper.WriteLine($"{nameof(expected)}: {expected}");
        helper.WriteLine($"{nameof(actual)}: {actual}");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(@"\\one\two", true)]
    [InlineData(@"\\\one\two", false)]
    [InlineData(@"", false)]
    [InlineData(null, false)]
    public void IsUnc_Test(string? input, bool isUnc)
    {
        Assert.Equal(isUnc, input.IsUnc());
    }

    [Theory]
    [InlineData("two", "one,two,three", true)]
    [InlineData("", "one,two,three", false)]
    [InlineData(null, "one,two,three", false)]
    public void In_Test(string? search, string input, bool isIn)
    {
        Assert.Equal(isIn, search.In(input));
    }

    [Theory]
    [InlineData("213-778-3064", true)]
    [InlineData("(213) 778-3064", true)]
    [InlineData("(213)7783064", true)]
    [InlineData("2137783064", true)]
    public void IsTelephoneNumber_Test(string input, bool isTelephoneNumber)
    {
        Assert.Equal(isTelephoneNumber, input.IsTelephoneNumber());
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("one", "eno")]
    public void Reverse_Test(string input, string expected)
    {
        var actual = input.Reverse();
        helper.WriteLine($"{nameof(input)}: {input}");
        helper.WriteLine($"{nameof(expected)}: {expected}");
        helper.WriteLine($"{nameof(actual)}: {actual}");
        Assert.Equal(expected, actual);
    }
}