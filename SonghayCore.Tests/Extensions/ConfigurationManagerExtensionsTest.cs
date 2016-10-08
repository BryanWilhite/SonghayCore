using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System.IO;
using System.Configuration;
using System.Xml.Linq;
using System.Linq;

namespace Songhay.Tests.Extensions
{
    /// <summary>
    /// Tests for <see cref="ConfigurationManagerExtensions"/>
    /// </summary>
    [TestClass]
    public class ConfigurationManagerExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("externalConfigurationFile", @"SonghayCore.Tests\Extensions\ConfigurationManagerExtensionsTest.xml")]
        public void ShouldGetConnectionStringSettings()
        {
            var projectsFolder = this.TestContext.ShouldGetProjectsFolder(this.GetType(), i =>
            {
                i[0] = i[0].Replace("Songhay", "SonghayCore");
                return i;
            });

            #region test properties:

            var externalConfigurationFile = this.TestContext.Properties["externalConfigurationFile"].ToString();
            externalConfigurationFile = Path.Combine(projectsFolder, externalConfigurationFile);
            this.TestContext.ShouldFindFile(externalConfigurationFile);

            #endregion

            var externalConfigurationDoc = XDocument.Load(externalConfigurationFile);
            var collection = externalConfigurationDoc.ToConnectionStringSettingsCollection();
            collection.OfType<ConnectionStringSettings>().ForEachInEnumerable(i => this.TestContext.WriteLine(i.ToString()));

            Assert.IsNotNull(collection, string.Format("The expected ConnectionStringSettingsCollection is not here."));
            Assert.IsTrue(collection.OfType<ConnectionStringSettings>().Any(), "The expected ConnectionStringSettings are not here.");
        }
    }
}
