﻿using Songhay.Extensions;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{

    /// <summary>
    /// Tests for <see cref="StringExtensions"/>
    /// </summary>
    public class StringExtensionsTests
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

        readonly ITestOutputHelper _testOutputHelper;
    }
}