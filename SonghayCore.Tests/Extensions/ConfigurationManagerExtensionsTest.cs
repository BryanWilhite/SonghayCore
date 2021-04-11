using Songhay.Extensions;
using System.IO;
using System.Configuration;
using System.Xml.Linq;
using System.Linq;
using Songhay.Models;
using Xunit.Abstractions;
using Xunit;

namespace Songhay.Tests.Extensions
{
    public class ConfigurationManagerExtensionsTest
    {
        public ConfigurationManagerExtensionsTest(ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }

        [Theory]
        [ProjectFileData(
            typeof(ConfigurationManagerExtensionsTest),
            new object[] {
                @"Data Source=|DataDirectory|\Chinook.dev.sqlite",
                "Chinook"
            },
            "../../../Extensions/ConfigurationManagerExtensionsTest.xml")]
        public void ShouldGetExternalConnectionStringSettings(string expectedConnectionString, string unqualifiedName, FileInfo externalConfigurationFileInfo)
        {
            /*
                FUNKYKB: `ConfigurationManager.AppSettings` is not returning expected results on Linux; `ConfigurationManager.OpenExeConfiguration` is used instead:
            */
            var configuration = ConfigurationManager
                .OpenExeConfiguration(this.GetType().Assembly.Location);
            Assert.NotNull(configuration);
            Assert.True(configuration.HasFile);

            var environmentName = configuration
                .AppSettings.Settings
                .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            var externalConfigurationDoc =
                XDocument.Load(externalConfigurationFileInfo.FullName);

            var collection = ConfigurationManager
                .ConnectionStrings
                .WithConnectionStringSettingsCollection(externalConfigurationDoc);
            Assert.NotEmpty(collection);
            Assert.True(collection.OfType<ConnectionStringSettings>().Any(), "The expected ConnectionStringSettings are not here.");
            collection.OfType<ConnectionStringSettings>().ForEachInEnumerable(i => this._testOutputHelper.WriteLine(i.ToString()));

            var name = collection.GetConnectionNameFromEnvironment(unqualifiedName, environmentName);
            this._testOutputHelper.WriteLine("name: {0}", name);

            var settings = collection.GetConnectionStringSettings(name);
            Assert.NotNull(settings);
            var actual = settings.ConnectionString;
            this._testOutputHelper.WriteLine("actual: {0}", actual);

            Assert.Equal(expectedConnectionString, actual);
        }
/*
        [Theory]
        [TestProperty("expectedSetting", "the external setting for DEV")]
        [TestProperty("externalConfigurationFile", @"Extensions\ConfigurationManagerExtensionsTest.xml")]
        [TestProperty("unqualifiedKey", "ex-setting")]
        public void ShouldGetExternalSetting()
        {
            var projectsFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 2);

            #region test properties:

            var expectedSetting = this.TestContext.Properties["expectedSetting"].ToString();
            this._testOutputHelper.WriteLine("expected: {0}", expectedSetting);

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
            this._testOutputHelper.WriteLine("appSettings keys:");
            Assert.True(collection.AllKeys.Any(), "The expected appSettings keys are not here.");
            collection.AllKeys.ForEachInEnumerable(i => this._testOutputHelper.WriteLine("key: {0}, value: {1}", i, collection[i]));

            var key = collection.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
            this._testOutputHelper.WriteLine("key: {0}", key);

            var actual = collection.GetSetting(key);
            this._testOutputHelper.WriteLine("actual: {0}", actual);

            Assert.Equal(expectedSetting, actual, "The expectedSetting is not here.");
        }

        [Theory]
        [TestProperty("expectedConnectionString", @"Data Source=|DataDirectory|\Northwind.dev.sqlite")]
        [TestProperty("unqualifiedName", "Northwind")]
        public void ShouldGetConnectionStringSettings()
        {

            #region test properties:

            var expectedConnectionString = this.TestContext.Properties["expectedConnectionString"].ToString();
            this._testOutputHelper.WriteLine("expected: {0}", expectedConnectionString);

            var unqualifiedName = this.TestContext.Properties["unqualifiedName"].ToString();

            #endregion

            var environmentName = ConfigurationManager
                .AppSettings
                .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            var name = ConfigurationManager
                .ConnectionStrings
                .GetConnectionNameFromEnvironment(unqualifiedName, environmentName);
            this._testOutputHelper.WriteLine("name: {0}", name);

            var settings = ConfigurationManager
                .ConnectionStrings
                .GetConnectionStringSettings(name);
            Assert.NotNull(settings, "The expected settings are not here.");
            var actual = settings.ConnectionString;
            this._testOutputHelper.WriteLine("actual: {0}", actual);

            Assert.Equal(expectedConnectionString, actual, "The expectedConnectionString is not here.");
        }

        [Theory]
        [TestProperty("expectedEnvironmentName", DeploymentEnvironment.DevelopmentEnvironmentName)]
        public void ShouldGetEnvironmentName()
        {
            var expectedEnvironmentName = this.TestContext.Properties["expectedEnvironmentName"].ToString();
            this._testOutputHelper.WriteLine("expected: {0}", expectedEnvironmentName);

            var actual = ConfigurationManager.AppSettings.GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            this._testOutputHelper.WriteLine("actual: {0}", actual);

            Assert.Equal(expectedEnvironmentName, actual, "The expectedEnvironmentName is not here.");
        }

        [Theory]
        [TestProperty("expectedKey", "dev.setting")]
        [TestProperty("unqualifiedKey", "setting")]
        public void ShouldGetKeyWithEnvironmentName()
        {
            var expectedKey = this.TestContext.Properties["expectedKey"].ToString();
            this._testOutputHelper.WriteLine("expected: {0}", expectedKey);
            var unqualifiedKey = this.TestContext.Properties["unqualifiedKey"].ToString();

            var environmentName = ConfigurationManager.AppSettings.GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            var actual = ConfigurationManager.AppSettings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
            this._testOutputHelper.WriteLine("actual: {0}", actual);

            Assert.Equal(expectedKey, actual, "The expectedKey is not here.");
        }

        [Theory]
        [TestProperty("expectedSetting", "the setting for DEV")]
        [TestProperty("unqualifiedKey", "setting")]
        public void ShouldGetSetting()
        {
            var expectedSetting = this.TestContext.Properties["expectedSetting"].ToString();
            this._testOutputHelper.WriteLine("expected: {0}", expectedSetting);
            var unqualifiedKey = this.TestContext.Properties["unqualifiedKey"].ToString();

            var environmentName = ConfigurationManager
                .AppSettings
                .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
            var key = ConfigurationManager.AppSettings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
            this._testOutputHelper.WriteLine("key: {0}", key);

            var actual = ConfigurationManager.AppSettings.GetSetting(key);
            this._testOutputHelper.WriteLine("actual: {0}", actual);

            Assert.Equal(expectedSetting, actual, "The expectedSetting is not here.");
        }
*/
        readonly ITestOutputHelper _testOutputHelper;
    }
}
