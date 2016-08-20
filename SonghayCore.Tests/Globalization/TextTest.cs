using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests
{
    using Songhay.Extensions;
    using Songhay.Globalization;

    /// <summary>
    /// Summary description for TextTest
    /// </summary>
    [TestClass]
    public class TextTest
    {
        [TestInitialize]
        public void ClearPreviousTestResults()
        {
            var directory = Directory.GetParent(TestContext.TestDir);

            directory.GetFiles()
                .OrderByDescending(f => f.LastAccessTime).Skip(1)
                .ForEachInEnumerable(f => f.Delete());

            directory.GetDirectories()
                .OrderByDescending(d => d.LastAccessTime).Skip(1)
                .ForEachInEnumerable(d => d.Delete(true));
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [Description("Should get string from char array.")]
        public void ShouldGetStringFromCharArray()
        {
            var charArray = new char[] { 'A', 'B' };
            var s = string.Join(string.Empty, charArray);
            Assert.AreEqual("AB", s, "The expected string is not here");
        }

        [TestMethod]
        [Description("Should split and join text.")]
        public void ShouldSplitAndJoinText()
        {
            var input = "one two three four five";
            var expected = "one two THREE four five";
            var actual = string.Join(" ", input.Split(' ')
                .Select(s => (s == "three") ? s.ToUpper() : s).ToArray());
            Assert.AreEqual(expected, actual, "The expected and actual strings are not equal.");
        }

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
