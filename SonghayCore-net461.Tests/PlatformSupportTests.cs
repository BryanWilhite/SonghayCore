using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Tests.Models;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
        [TestProperty("projectFileNamespace", "http://schemas.microsoft.com/developer/msbuild/2003")]
        [TestProperty("projectsDirectory", "SonghayCore")]
        [TestProperty("rootPlatform", PlatformConstants.net461)]
        [TestProperty("targetPlatform", PlatformConstants.netstandard20)]
        public void ShouldFindMissingCompileIncludes()
        {
            #region test properties:

            XNamespace projectFileNamespace = this.TestContext.Properties["projectFileNamespace"].ToString();
            var projectsDirectory = this.TestContext.Properties["projectsDirectory"].ToString();
            var rootPlatform = this.TestContext.Properties["rootPlatform"].ToString();
            var targetPlatform = this.TestContext.Properties["targetPlatform"].ToString();

            #endregion

            var info = this.TestContext.ShouldGetAssemblyDirectoryInfo(this.GetType());
            var parentInfo = info.Parent?.Parent?.Parent;
            Assert.IsNotNull(parentInfo, "The expected parent Directory Info is not here.");
            Assert.AreEqual(parentInfo.Name, projectsDirectory);

            var baseProjectFile = Path.Combine(parentInfo.FullName,
                $"{projectsDirectory}-{rootPlatform}",
                $"{projectsDirectory}-{rootPlatform}.csproj");
            this.TestContext.ShouldFindFile(baseProjectFile);

            var baseProjectDoc = XDocument.Load(baseProjectFile);
            var baseProjectCompiles = baseProjectDoc.Root.Descendants(projectFileNamespace + "Compile").ToArray();
            Assert.IsTrue(baseProjectCompiles.Any(), "The expected Compile elements are not here.");
            this.TestContext.WriteLine("Base Project Compile Elements:");
            baseProjectCompiles.ForEachInEnumerable(i => this.TestContext.WriteLine(i.Attribute("Include").Value));
        }
    }
}
