using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;

namespace Songhay.Tests
{
    [TestClass]
    public class PlatformSupportTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldFindMissingCompileIncludes()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 3);
        }
    }
}
