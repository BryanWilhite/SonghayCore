using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System.IO;
using System.Configuration;
using System.Xml.Linq;
using System.Linq;
using Songhay.Models;

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
        [TestProperty("expectedConnectionString", @"Data Source=|DataDirectory|\Chinook.dev.sqlite")]
        [TestProperty("externalConfigurationFile", @"SonghayCore.Tests\Extensions\ConfigurationManagerExtensionsTest.xml")]
        [TestProperty("unqualifiedName", "Chinook")]
        public void ShouldGetExternalConnectionStringSettings()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);

            #region test properties:

            var expectedConnectionString = this.TestContext.Properties["expectedConnectionString"].ToString();
            this.TestContext.WriteLine("expected: {0}", expectedConnectionString);

            var externalConfigurationFile = this.TestContext.Properties["externalConfigurationFile"].ToString();
            externalConfigurationFile = Path.Combine(projectsFolder, externalConfigurationFile);
            this.TestContext.ShouldFindFile(externalConfigurationFile);

            var unqualifiedName = this.TestContext.Properties["unqualifiedName"].ToString();

            #endregion

            var environmentName = ConfigurationManager
                .AppSettings
                .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            var externalConfigurationDoc = XDocument.Load(externalConfigurationFile);

            var collection = ConfigurationManager
                .ConnectionStrings
                .WithConnectionStringSettingsCollection(externalConfigurationDoc);
            Assert.IsNotNull(collection, string.Format("The expected ConnectionStringSettingsCollection is not here."));
            Assert.IsTrue(collection.OfType<ConnectionStringSettings>().Any(), "The expected ConnectionStringSettings are not here.");
            collection.OfType<ConnectionStringSettings>().ForEachInEnumerable(i => this.TestContext.WriteLine(i.ToString()));

            var name = collection.GetConnectionNameFromEnvironment(unqualifiedName, environmentName);
            this.TestContext.WriteLine("name: {0}", name);

            var settings = collection.GetConnectionStringSettings(name);
            Assert.IsNotNull(settings, "The expected settings are not here.");
            var actual = settings.ConnectionString;
            this.TestContext.WriteLine("actual: {0}", actual);

            Assert.AreEqual(expectedConnectionString, actual, "The expectedConnectionString is not here.");
        }

        [TestMethod]
        [TestProperty("expectedSetting", "the external setting for DEV")]
        [TestProperty("externalConfigurationFile", @"SonghayCore.Tests\Extensions\ConfigurationManagerExtensionsTest.xml")]
        [TestProperty("unqualifiedName", "ex-setting")]
        public void ShouldGetExternalSetting()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);

            #region test properties:

            var expectedSetting = this.TestContext.Properties["expectedSetting"].ToString();
            this.TestContext.WriteLine("expected: {0}", expectedSetting);

            var externalConfigurationFile = this.TestContext.Properties["externalConfigurationFile"].ToString();
            externalConfigurationFile = Path.Combine(projectsFolder, externalConfigurationFile);
            this.TestContext.ShouldFindFile(externalConfigurationFile);

            var unqualifiedKey = this.TestContext.Properties["unqualifiedKey"].ToString();

            #endregion

            var environmentName = ConfigurationManager
                .AppSettings
                .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");

            var externalConfigurationDoc = XDocument.Load(externalConfigurationFile);
            var collection = ConfigurationManager
                .AppSettings
                .WithAppSettings(externalConfigurationDoc);
            this.TestContext.WriteLine("appSettings keys:");
            Assert.IsTrue(collection.AllKeys.Any(), "The expected appSettings keys are not here.");
            collection.AllKeys.ForEachInEnumerable(i => this.TestContext.WriteLine("key: {0}, value: {1}", i, collection[i]));

            var key = collection.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
            this.TestContext.WriteLine("key: {0}", key);

            var actual = collection.GetSetting(key);
            this.TestContext.WriteLine("actual: {0}", actual);

            Assert.AreEqual(expectedSetting, actual, "The expectedSetting is not here.");
        }

        [TestMethod]
        [TestProperty("expectedConnectionString", @"Data Source=|DataDirectory|\Northwind.dev.sqlite")]
        [TestProperty("unqualifiedName", "Northwind")]
        public void ShouldGetConnectionStringSettings()
        {

            #region test properties:

            var expectedConnectionString = this.TestContext.Properties["expectedConnectionString"].ToString();
            this.TestContext.WriteLine("expected: {0}", expectedConnectionString);

            var unqualifiedName = this.TestContext.Properties["unqualifiedName"].ToString();

            #endregion

            var environmentName = ConfigurationManager
                .AppSettings
                .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            var name = ConfigurationManager
                .ConnectionStrings
                .GetConnectionNameFromEnvironment(unqualifiedName, environmentName);
            this.TestContext.WriteLine("name: {0}", name);

            var settings = ConfigurationManager
                .ConnectionStrings
                .GetConnectionStringSettings(name);
            Assert.IsNotNull(settings, "The expected settings are not here.");
            var actual = settings.ConnectionString;
            this.TestContext.WriteLine("actual: {0}", actual);

            Assert.AreEqual(expectedConnectionString, actual, "The expectedConnectionString is not here.");
        }

        [TestMethod]
        [TestProperty("expectedEnvironmentName", DeploymentEnvironment.DevelopmentEnvironmentName)]
        public void ShouldGetEnvironmentName()
        {
            var expectedEnvironmentName = this.TestContext.Properties["expectedEnvironmentName"].ToString();
            this.TestContext.WriteLine("expected: {0}", expectedEnvironmentName);

            var actual = ConfigurationManager.AppSettings.GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            this.TestContext.WriteLine("actual: {0}", actual);

            Assert.AreEqual(expectedEnvironmentName, actual, "The expectedEnvironmentName is not here.");
        }

        [TestMethod]
        [TestProperty("expectedKey", "dev.setting")]
        [TestProperty("unqualifiedKey", "setting")]
        public void ShouldGetKeyWithEnvironmentName()
        {
            var expectedKey = this.TestContext.Properties["expectedKey"].ToString();
            this.TestContext.WriteLine("expected: {0}", expectedKey);
            var unqualifiedKey = this.TestContext.Properties["unqualifiedKey"].ToString();

            var environmentName = ConfigurationManager.AppSettings.GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            var actual = ConfigurationManager.AppSettings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
            this.TestContext.WriteLine("actual: {0}", actual);

            Assert.AreEqual(expectedKey, actual, "The expectedKey is not here.");
        }

        [TestMethod]
        [TestProperty("expectedSetting", "the setting for DEV")]
        [TestProperty("unqualifiedKey", "setting")]
        public void ShouldGetSetting()
        {
            var expectedSetting = this.TestContext.Properties["expectedSetting"].ToString();
            this.TestContext.WriteLine("expected: {0}", expectedSetting);
            var unqualifiedKey = this.TestContext.Properties["unqualifiedKey"].ToString();

            var environmentName = ConfigurationManager
                .AppSettings
                .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            var key = ConfigurationManager.AppSettings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
            this.TestContext.WriteLine("key: {0}", key);

            var actual = ConfigurationManager.AppSettings.GetSetting(key);
            this.TestContext.WriteLine("actual: {0}", actual);

            Assert.AreEqual(expectedSetting, actual, "The expectedSetting is not here.");
        }
    }
}
