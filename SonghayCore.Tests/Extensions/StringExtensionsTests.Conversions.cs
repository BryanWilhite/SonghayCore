using Songhay.Extensions;
using Xunit;

namespace Songhay.Tests.Extensions;

public partial class StringExtensionsTests
{
    [Theory]
    [InlineData("", '\0', null)]
    [InlineData("", 'x', null)]
    [InlineData("kinté space", '\0', "kint space")]
    [InlineData("|kinté space|", '|', "kint space")]
    [InlineData("|kinté|space|", '\0', "|kint|space|")]
    public void ToAsciiLettersWithSpacer_Test(string input, char spacer, string expected)
    {
        var actual = input.ToAsciiLettersWithSpacer(spacer);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("“How to improve your site’s UX” and other Tweeted Links…", "how-to-improve-your-site-s-ux-and-other-tweeted-links")]
    [InlineData("My Angular JS 1.x single-page layout", "my-angular-js-1-x-single-page-layout")]
    [InlineData("Put F# on the TODO list?", "put-f-on-the-todo-list")]
    [InlineData("Silverlight Page Navigating with MVVM Light Messaging and Songhay NavigationBookmarkData",
        "silverlight-page-navigating-with-mvvm-light-messaging-and-songhay-navigationbookmarkdata")]
    public void ToBlogSlug_Test(string input, string expectedOutput)
    {
        var slug = input.ToBlogSlug();
        _testOutputHelper.WriteLine("slug: {0}", slug);
        Assert.True(slug.EqualsInvariant(expectedOutput));
    }

    [Theory]
    [InlineData("$.05", "nil", "nil")]
    [InlineData("$.45", "0", "0")]
    [InlineData("$1,000.45", "0", "1000")]
    public void ToIntString_Test(string input, string defaultValue, string expected)
    {
        var actual = input.ToIntString(defaultValue);
        _testOutputHelper.WriteLine($"{nameof(input)}: {input}");
        _testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
        _testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("number: $1234.45US", "1234.45")]
    [InlineData("number: -$1234.45US", "-1234.45")]
    [InlineData("", "0")]
    [InlineData(null, "0")]
    public void ToNumericString_Test(string? input, string? expected)
    {
        var actual = input.ToNumericString();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("number: $1234.45US", "[default]", "1234.45")]
    [InlineData("number: -$1234.45US", "[default]", "-1234.45")]
    [InlineData("", "[default]", "[default]")]
    [InlineData(null, "[default]", "[default]")]
    public void ToNumericString_WithDefaultValue_Test(string? input, string? defaultValue, string? expected)
    {
        var actual = input.ToNumericString(defaultValue);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("oneTwoThree", "OneTwoThree")]
    public void ToPascalCase_Test(string input, string expected)
    {
        var actual = input.ToPascalCase();
        _testOutputHelper.WriteLine($"{nameof(input)}: {input}");
        _testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
        _testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("OneTwoThree", "one_two_three")]
    public void ToSnakeCase_Test(string input, string expected)
    {
        var actual = input.ToSnakeCase();
        _testOutputHelper.WriteLine($"{nameof(input)}: {input}");
        _testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
        _testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(10, "Lorem ipsum dolor", "ipsum")]
    public void ToSubstringInContext_Test(int contextLength, string input, string searchText)
    {
        var actual = input.ToSubstringInContext(searchText, contextLength).ToReferenceTypeValueOrThrow();

        Assert.True(actual.Contains(searchText), "The expected search text is not here.");
        Assert.True(actual.Length >= contextLength, "The expected text length is not here.");

        contextLength = (contextLength / 2);
        actual = input.ToSubstringInContext(searchText, contextLength).ToReferenceTypeValueOrThrow();

        Assert.True(actual.Contains(searchText), "The expected search text is not here.");
        Assert.True(actual.Length == contextLength, "The expected text length is not here.");
    }

    [Theory]
    [InlineData("This is the long thing.", 8, "This is…")]
    [InlineData("one", 3, "one")]
    public void Truncate_Test(string input, int length, string expected)
    {
        var actual = input.Truncate(length);
        _testOutputHelper.WriteLine($"{nameof(input)}: {input}");
        _testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
        _testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
        Assert.Equal(expected, actual);
    }
}
