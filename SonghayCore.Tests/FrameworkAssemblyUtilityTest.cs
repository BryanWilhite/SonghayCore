using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;

namespace Songhay.Tests
{
    [TestClass]
    public class FrameworkAssemblyUtilityTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("fileSegment", @"..\..\content\FrameworkAssemblyUtilityTest-ShouldGetPathFromAssembly.json")]
        public void ShouldGetPathFromAssembly()
        {
            #region test properties:

            var fileSegment = this.TestContext.Properties["fileSegment"].ToString();

            #endregion

            var actualPath = FrameworkAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, fileSegment);
            this.TestContext.ShouldFindFile(actualPath);
        }
    }
}
