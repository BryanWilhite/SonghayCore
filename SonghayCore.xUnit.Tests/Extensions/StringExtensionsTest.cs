using Songhay.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{

    /// <summary>
    /// Tests for <see cref="StringExtensions"/>
    /// </summary>
    public class StringExtensionsTest
    {
        public StringExtensionsTest(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Theory]
        [InlineData(10, "Lorem ipsum dolor", "ipsum")]
        public void ShouldGetSubstringInContext(int contextLength, string input, string searchText)
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
        [InlineData("Silverlight Page Navigating with MVVM Light Messaging and Songhay NavigationBookmarkData")]
        public void ShouldTransformToBlogSlug(string input)
        {
            var slug = input.ToBlogSlug();
            this._testOutputHelper.WriteLine("slug: {0}", slug);
        }

        readonly ITestOutputHelper _testOutputHelper;
    }
}
