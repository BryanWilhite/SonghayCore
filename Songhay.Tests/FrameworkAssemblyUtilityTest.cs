using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System.IO;
using System.Linq;

namespace Songhay.Tests
{
    [TestClass]
    public class FrameworkAssemblyUtilityTest
    {
        /// <summary>
        /// Initializes the test.
        /// </summary>
        [TestInitialize]
        public void InitializeTest()
        {
            #region remove previous test results:

            var directory = Directory.GetParent(TestContext.TestDir);

            directory.GetFiles()
                .OrderByDescending(f => f.LastAccessTime).Skip(1)
                .ForEachInEnumerable(f => f.Delete());

            directory.GetDirectories()
                .OrderByDescending(d => d.LastAccessTime).Skip(1)
                .ForEachInEnumerable(d => d.Delete(true));

            #endregion
        }

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
