using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Tests.Models;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Songhay.Tests
{
    [TestClass]
    public class PlatformSupportTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("projectsDirectory", "SonghayCore")]
        [TestProperty("rootPlatform", PlatformConstants.netstandard20)]
        [TestProperty("targetPlatform", PlatformConstants.netstandard14)]
        public void ShouldDiffCompileIncludesForNetCore()
        {
            #region test properties:

            var projectsDirectory = this.TestContext.Properties["projectsDirectory"].ToString();
            var rootPlatform = this.TestContext.Properties["rootPlatform"].ToString();
            var targetPlatform = this.TestContext.Properties["targetPlatform"].ToString();

            #endregion

            var info = this.TestContext.ShouldGetAssemblyDirectoryInfo(this.GetType());
            var parentInfo = info.Parent?.Parent?.Parent;
            Assert.IsNotNull(parentInfo, "The expected parent Directory Info is not here.");
            Assert.AreEqual(parentInfo.Name, projectsDirectory);

            var coreProjectInfo = parentInfo.GetDirectories().First(i => i.Name == projectsDirectory);
            var coreFiles = coreProjectInfo
                .EnumerateFiles("*.cs", SearchOption.AllDirectories)
                .Select(i => i.FullName.Replace(coreProjectInfo.Parent.FullName, "..").ToLowerInvariant());

            //coreFiles.ForEachInEnumerable(i => this.TestContext.WriteLine(i));

            var targetProjectFile = Path.Combine(parentInfo.FullName,
                $"{projectsDirectory}-{targetPlatform}",
                $"{projectsDirectory}-{targetPlatform}.csproj");
            this.TestContext.ShouldFindFile(targetProjectFile);

            var targetProjectDoc = XDocument.Load(targetProjectFile);
            var targetProjectCompiles = targetProjectDoc.Root.Descendants("Compile").ToArray();
            Assert.IsTrue(targetProjectCompiles.Any(), "The expected target Compile elements are not here.");

            var exceptions = coreFiles.Except(targetProjectCompiles.Select(i => i.Attribute("Include").Value.ToLowerInvariant()));
            if (exceptions.Any())
            {
                this.TestContext.WriteLine("The core project has Compile elements that are not in the framework.");
                this.TestContext.WriteLine($"{exceptions.Count()} exceptions found:");
                exceptions.OrderBy(i => i).ForEachInEnumerable(i => this.TestContext.WriteLine(i));
            }
            else
            {
                this.TestContext.WriteLine("No framework-base exceptions found.");
            }

            this.TestContext.WriteLine(string.Empty);

            exceptions = targetProjectCompiles.Select(i => i.Attribute("Include").Value.ToLowerInvariant()).Except(coreFiles);
            if (exceptions.Any())
            {
                this.TestContext.WriteLine("The framework project has Compile elements that are not in the core.");
                this.TestContext.WriteLine($"{exceptions.Count()} exceptions found:");
                exceptions.OrderBy(i => i).ForEachInEnumerable(i => this.TestContext.WriteLine(i));
            }
            else
            {
                this.TestContext.WriteLine("No base-framework exceptions found.");
            }
        }

        [TestMethod]
        [TestProperty("projectFileNamespace", "http://schemas.microsoft.com/developer/msbuild/2003")]
        [TestProperty("projectsDirectory", "SonghayCore")]
        [TestProperty("rootPlatform", PlatformConstants.net461)]
        [TestProperty("targetPlatform", PlatformConstants.net452)]
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

        [TestMethod]
        [TestProperty("projectFileNamespace", "http://schemas.microsoft.com/developer/msbuild/2003")]
        [TestProperty("projectsDirectory", "SonghayCore")]
        public void ShouldDiffCompileIncludesBetweenCoreAndFramework()
        {
            #region test properties:

            XNamespace projectFileNamespace = this.TestContext.Properties["projectFileNamespace"].ToString();
            var projectsDirectory = this.TestContext.Properties["projectsDirectory"].ToString();

            #endregion

            var info = this.TestContext.ShouldGetAssemblyDirectoryInfo(this.GetType());
            var parentInfo = info.Parent?.Parent?.Parent;
            Assert.IsNotNull(parentInfo, "The expected parent Directory Info is not here.");
            Assert.AreEqual(parentInfo.Name, projectsDirectory);

            var coreProjectInfo = parentInfo.GetDirectories().First(i => i.Name == projectsDirectory);
            var coreFiles = coreProjectInfo
                .EnumerateFiles("*.cs", SearchOption.AllDirectories)
                .Select(i => i.FullName.Replace(coreProjectInfo.Parent.FullName, "..").ToLowerInvariant());

            //coreFiles.ForEachInEnumerable(i => this.TestContext.WriteLine(i));

            var framework20ProjectFile = Path.Combine(parentInfo.FullName,
                $"{projectsDirectory.Replace("Core", string.Empty)}",
                $"{projectsDirectory.Replace("Core", string.Empty)}.csproj");
            this.TestContext.ShouldFindFile(framework20ProjectFile);

            var framework20ProjectDoc = XDocument.Load(framework20ProjectFile);
            var framework20ProjectCompiles = framework20ProjectDoc.Root.Descendants(projectFileNamespace + "Compile").ToArray();
            Assert.IsTrue(framework20ProjectCompiles.Any(), "The expected framework 2.0 Compile elements are not here.");

            var frameworkProjectFile = Path.Combine(parentInfo.FullName,
                $"{projectsDirectory}-{PlatformConstants.net461}",
                $"{projectsDirectory}-{PlatformConstants.net461}.csproj");
            this.TestContext.ShouldFindFile(frameworkProjectFile);

            var frameworkProjectDoc = XDocument.Load(frameworkProjectFile);
            var frameworkProjectCompiles = frameworkProjectDoc.Root.Descendants(projectFileNamespace + "Compile").ToArray();
            Assert.IsTrue(frameworkProjectCompiles.Any(), "The expected framework Compile elements are not here.");

            var exceptions = coreFiles.Except(frameworkProjectCompiles.Union(framework20ProjectCompiles)
                .Select(i => i.Attribute("Include").Value.ToLowerInvariant()));
            if (exceptions.Any())
            {
                this.TestContext.WriteLine("The core project has Compile elements that are not in the framework.");
                this.TestContext.WriteLine($"{exceptions.Count()} exceptions found:");
                exceptions.OrderBy(i => i).ForEachInEnumerable(i => this.TestContext.WriteLine(i));
            }
            else
            {
                this.TestContext.WriteLine("No framework-base exceptions found.");
            }

            this.TestContext.WriteLine(string.Empty);

            exceptions = frameworkProjectCompiles.Union(framework20ProjectCompiles)
                .Select(i => i.Attribute("Include").Value.ToLowerInvariant()).Except(coreFiles);
            if (exceptions.Any())
            {
                this.TestContext.WriteLine("The framework project has Compile elements that are not in the core.");
                this.TestContext.WriteLine($"{exceptions.Count()} exceptions found:");
                exceptions.OrderBy(i => i).ForEachInEnumerable(i => this.TestContext.WriteLine(i));
            }
            else
            {
                this.TestContext.WriteLine("No base-framework exceptions found.");
            }
        }
    }
}
