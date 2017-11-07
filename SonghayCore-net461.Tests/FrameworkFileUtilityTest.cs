using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Songhay.Tests
{
    [TestClass]
    public class FrameworkFileUtilityTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("expectedPath", @"foo\bar\my-file.json")]
        [TestProperty("path", @"/\foo\bar\my-file.json")]
        public void ShouldTrimLeadingDirectorySeparatorChars()
        {
            #region test properties:

            var expectedPath = this.TestContext.Properties["expectedPath"].ToString();
            var path = this.TestContext.Properties["path"].ToString();

            #endregion

            var actualPath = FrameworkFileUtility.TrimLeadingDirectorySeparatorChars(path);
            Assert.AreEqual(expectedPath, actualPath, "The expected path is not here.");
        }
    }
}
