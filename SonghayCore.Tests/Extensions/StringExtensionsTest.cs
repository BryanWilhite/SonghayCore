using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System;

namespace Songhay.Tests.Extensions
{

    /// <summary>
    /// Tests for <see cref="StringExtensions"/>
    /// </summary>
    [TestClass]
    public class StringExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("root1", @"z:\one")]
        [TestProperty("root2", @"/home/one")]
        [TestProperty("path1", @"/two/three/four/")]
        [TestProperty("path2", @"\two\three\four")]
        public void ShouldConvertToCombinedFullPath()
        {
            #region test properties

            var root1 = this.TestContext.Properties["root1"].ToString();
            var root2 = this.TestContext.Properties["root2"].ToString();
            var path1 = this.TestContext.Properties["path1"].ToString();
            var path2 = this.TestContext.Properties["path2"].ToString();

            #endregion

            this.TestContext.WriteLine(root1.ToCombinedPath(path1));
            this.TestContext.WriteLine(root1.ToCombinedPath(path2));
            this.TestContext.WriteLine(root2.ToCombinedPath(path1));
            this.TestContext.WriteLine(root2.ToCombinedPath(path2));
        }

        [TestMethod]
        [TestProperty("contextLength", "10")]
        [TestProperty("input", "Lorem ipsum dolor")]
        [TestProperty("searchText", "ipsum")]
        public void ShouldGetSubstringInContext()
        {
            #region test properties:

            var contextLength = Convert.ToInt32(this.TestContext.Properties["contextLength"]);
            var input = this.TestContext.Properties["input"].ToString();
            var searchText = this.TestContext.Properties["searchText"].ToString();

            #endregion

            var actual = input.ToSubstringInContext(searchText, contextLength);

            Assert.IsTrue(actual.Contains(searchText), "The expected search text is not here.");
            Assert.IsTrue(actual.Length >= contextLength, "The expected text length is not here.");

            contextLength = (contextLength / 2);
            actual = input.ToSubstringInContext(searchText, contextLength);

            Assert.IsTrue(actual.Contains(searchText), "The expected search text is not here.");
            Assert.IsTrue(actual.Length == contextLength, "The expected text length is not here.");
        }

        [TestMethod]
        [TestProperty("input", "Silverlight Page Navigating with MVVM Light Messaging and Songhay NavigationBookmarkData")]
        public void ShouldTransformToBlogSlug()
        {
            var input = this.TestContext.Properties["input"].ToString();

            var slug = input.ToBlogSlug();
            this.TestContext.WriteLine("slug: {0}", slug);
        }
    }
}
