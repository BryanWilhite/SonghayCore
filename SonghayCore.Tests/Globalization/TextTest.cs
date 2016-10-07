using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Songhay.Tests
{
    using Songhay.Globalization;

    /// <summary>
    /// Summary description for TextTest
    /// </summary>
    [TestClass]
    public class TextTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [Description("Should render title case.")]
        public void ShouldRenderTitleCase()
        {
            var input = "over the dale and between the two trees";
            var expected = "Over the Dale and between the Two Trees";
            var actual = TextInfoUtility.ToTitleCase(input);
            Assert.AreEqual(expected, actual, "The expected and actual strings are not equal.");

            input = "this, yes, but also that!";
            expected = "This, Yes, but also That!";
            actual = TextInfoUtility.ToTitleCase(input);
            Assert.AreEqual(expected, actual, "The expected and actual strings are not equal.");

            input = "golfer versus boxer";
            expected = "Golfer versus Boxer";
            actual = TextInfoUtility.ToTitleCase(input);
            Assert.AreEqual(expected, actual, "The expected and actual strings are not equal.");
        }

        [TestMethod]
        [Description("Should run regular expression.")]
        public void ShouldRunRegex()
        {
            var s = "she's here!";
            s = Regex.Replace(s, @"(\w)'(\w)", "$1’$2");
            StringAssert.Contains(s, "’", "The  expected character is not here.");
        }
    }
}
