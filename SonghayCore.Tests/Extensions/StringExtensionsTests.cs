using Songhay.Extensions;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{
    public partial class StringExtensionsTests
    {
        public StringExtensionsTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [InlineData(@"{ ""my"": ""json"" }")]
        public void EscapeInterpolation_Test(string input)
        {
            Assert.Throws<FormatException>(() => this._testOutputHelper.WriteLine(input, 0));
            this._testOutputHelper.WriteLine(input.EscapeInterpolation(), 0);
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
            this._testOutputHelper.WriteLine($"{nameof(input)}: {input}");
            this._testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
            this._testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", "nil", "nil")]
        [InlineData("one", "nil", @"""one""")]
        public void InDoubleQuotesOrDefault_Test(string input, string defaultValue, string expected)
        {
            var actual = input.InDoubleQuotesOrDefault(defaultValue);
            this._testOutputHelper.WriteLine($"{nameof(input)}: {input}");
            this._testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
            this._testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(@"\\one\two", true)]
        [InlineData(@"\\\one\two", false)]
        [InlineData(@"", false)]
        [InlineData(null, false)]
        public void IsUnc_Test(string input, bool isUnc)
        {
            Assert.Equal(isUnc, input.IsUnc());
        }

        [Theory]
        [InlineData("two", "one,two,three", true)]
        [InlineData("", "one,two,three", false)]
        [InlineData(null, "one,two,three", false)]
        public void In_Test(string search, string input, bool isIn)
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
            this._testOutputHelper.WriteLine($"{nameof(input)}: {input}");
            this._testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
            this._testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
            Assert.Equal(expected, actual);
        }

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
        [InlineData("$.05", "nil", "nil")]
        [InlineData("$.45", "0", "0")]
        [InlineData("$1,000.45", "0", "1000")]
        public void ToIntString_Test(string input, string defaultValue, string expected)
        {
            var actual = input.ToIntString(defaultValue);
            this._testOutputHelper.WriteLine($"{nameof(input)}: {input}");
            this._testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
            this._testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("oneTwoThree", "OneTwoThree")]
        public void ToPascalCase_Test(string input, string expected)
        {
            var actual = input.ToPascalCase();
            this._testOutputHelper.WriteLine($"{nameof(input)}: {input}");
            this._testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
            this._testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("OneTwoThree", "one_two_three")]
        public void ToSnakeCase_Test(string input, string expected)
        {
            var actual = input.ToSnakeCase();
            this._testOutputHelper.WriteLine($"{nameof(input)}: {input}");
            this._testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
            this._testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(10, "Lorem ipsum dolor", "ipsum")]
        public void ToSubstringInContext_Test(int contextLength, string input, string searchText)
        {
            var actual = input.ToSubstringInContext(searchText, contextLength);

            Assert.True(actual.Contains(searchText), "The expected search text is not here.");
            Assert.True(actual.Length >= contextLength, "The expected text length is not here.");

            contextLength = (contextLength / 2);
            actual = input.ToSubstringInContext(searchText, contextLength);

            Assert.True(actual.Contains(searchText), "The expected search text is not here.");
            Assert.True(actual.Length == contextLength, "The expected text length is not here.");
        }

        [Theory]
        [InlineData("“How to improve your site’s UX” and other Tweeted Links…", "how-to-improve-your-site-s-ux-and-other-tweeted-links")]
        [InlineData("My Angular JS 1.x single-page layout", "my-angular-js-1-x-single-page-layout")]
        [InlineData("Put F# on the TODO list?", "put-f-on-the-todo-list")]
        [InlineData("Silverlight Page Navigating with MVVM Light Messaging and Songhay NavigationBookmarkData",
            "silverlight-page-navigating-with-mvvm-light-messaging-and-songhay-navigationbookmarkdata")]
        public void TransformToBlogSlug_Test(string input, string expectedOutput)
        {
            var slug = input.ToBlogSlug();
            this._testOutputHelper.WriteLine("slug: {0}", slug);
            Assert.True(slug.EqualsInvariant(expectedOutput));
        }

        [Theory]
        [InlineData("This is the long thing.", 8, "This is…")]
        [InlineData("one", 3, "one")]
        public void Truncate_Test(string input, int length, string expected)
        {
            var actual = input.Truncate(length);
            this._testOutputHelper.WriteLine($"{nameof(input)}: {input}");
            this._testOutputHelper.WriteLine($"{nameof(expected)}: {expected}");
            this._testOutputHelper.WriteLine($"{nameof(actual)}: {actual}");
            Assert.Equal(expected, actual);
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
