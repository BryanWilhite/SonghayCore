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
        [TestProperty("targetPlatform", PlatformConstants.net40)]
        public void ShouldDiffCompileIncludesForNetFramework()
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
            Assert.IsTrue(baseProjectCompiles.Any(), "The expected base Compile elements are not here.");

            var targetProjectFile = Path.Combine(parentInfo.FullName,
                $"{projectsDirectory}-{targetPlatform}",
                $"{projectsDirectory}-{targetPlatform}.csproj");
            this.TestContext.ShouldFindFile(targetProjectFile);

            var targetProjectDoc = XDocument.Load(targetProjectFile);
            var targetProjectCompiles = targetProjectDoc.Root.Descendants(projectFileNamespace + "Compile").ToArray();
            Assert.IsTrue(targetProjectCompiles.Any(), "The expected target Compile elements are not here.");
            var exceptions = baseProjectCompiles.Select(i => i.Attribute("Include").Value.ToLowerInvariant())
                .Except(targetProjectCompiles.Select(i => i.Attribute("Include").Value.ToLowerInvariant()));
            if (exceptions.Any())
            {
                this.TestContext.WriteLine("The base project has Compile elements that are not in the target.");
                this.TestContext.WriteLine($"{exceptions.Count()} exceptions found:");
                exceptions.OrderBy(i => i).ForEachInEnumerable(i => this.TestContext.WriteLine(i));
            }
            else
            {
                this.TestContext.WriteLine("No target-base exceptions found.");
            }

            this.TestContext.WriteLine(string.Empty);

            exceptions = targetProjectCompiles.Select(i => i.Attribute("Include").Value.ToLowerInvariant())
                .Except(baseProjectCompiles.Select(i => i.Attribute("Include").Value.ToLowerInvariant()));
            if (exceptions.Any())
            {
                this.TestContext.WriteLine("The target project has Compile elements that are not in the base.");
                this.TestContext.WriteLine($"{exceptions.Count()} exceptions found:");
                exceptions.OrderBy(i => i).ForEachInEnumerable(i => this.TestContext.WriteLine(i));
            }
            else
            {
                this.TestContext.WriteLine("No base-target exceptions found.");
            }
        }
    }
}
