using System.Configuration;
using System.Xml.Linq;
using Songhay.Models;

namespace Songhay.Tests.Extensions;

public class ConfigurationManagerExtensionsTests(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [ProjectFileData(
        typeof(ConfigurationManagerExtensionsTests),
        [
            @"Data Source=|DataDirectory|\Chinook.dev.sqlite",
            "Chinook"
        ],
        "../../../Extensions/ConfigurationManagerExtensionsTests.xml")]
    public void ShouldGetExternalConnectionStringSettings(string expectedConnectionString, string unqualifiedName, FileInfo externalConfigurationFileInfo)
    {
        /*
            FUNKYKB: `ConfigurationManager.AppSettings` is not returning expected results on Linux; `ConfigurationManager.OpenExeConfiguration` is used instead:
        */
        var configuration = ConfigurationManager
            .OpenExeConfiguration(GetType().Assembly.Location);

        Assert.NotNull(configuration);
        Assert.True(configuration.HasFile);

        var environmentName = configuration
            .AppSettings.Settings
            .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
        var externalConfigurationDoc =
            XDocument.Load(externalConfigurationFileInfo.FullName);

        var collection = ConfigurationManager
            .ConnectionStrings
            .WithConnectionStringSettingsCollection(externalConfigurationDoc)
            .ToReferenceTypeValueOrThrow();

        Assert.NotEmpty(collection);
        Assert.True(collection.OfType<ConnectionStringSettings>().Any(), "The expected ConnectionStringSettings are not here.");
        collection.OfType<ConnectionStringSettings>().ForEachInEnumerable(i => testOutputHelper.WriteLine(i.ToString()));

        var name = collection.GetConnectionNameFromEnvironment(unqualifiedName, environmentName);
        testOutputHelper.WriteLine("name: {0}", name);

        var settings = collection.GetConnectionStringSettings(name).ToReferenceTypeValueOrThrow();

        var actual = settings.ConnectionString;
        testOutputHelper.WriteLine("actual: {0}", actual);

        Assert.Equal(expectedConnectionString, actual);
    }

    [Theory]
    [ProjectFileData(
        typeof(ConfigurationManagerExtensionsTests),
        [
            "the external setting for DEV",
            "ex-setting"
        ],
        "../../../Extensions/ConfigurationManagerExtensionsTests.xml")]
    public void ShouldGetExternalSetting(string expectedSetting, string unqualifiedKey, FileInfo externalConfigurationFileInfo)
    {
        var configuration = ConfigurationManager
            .OpenExeConfiguration(GetType().Assembly.Location);
        Assert.NotNull(configuration);
        Assert.True(configuration.HasFile);

        var environmentName = configuration
            .AppSettings.Settings
            .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");

        var externalConfigurationDoc =
            XDocument.Load(externalConfigurationFileInfo.FullName);

        var collection = configuration
            .AppSettings.Settings
            .WithAppSettings(externalConfigurationDoc)
            .ToReferenceTypeValueOrThrow();

        testOutputHelper.WriteLine("appSettings keys:");

        Assert.True(collection.AllKeys.Any(), "The expected appSettings keys are not here.");
        collection.AllKeys.ForEachInEnumerable(i => testOutputHelper.WriteLine("key: {0}, value: {1}", i, collection[i]));

        var key = collection.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
        testOutputHelper.WriteLine("key: {0}", key);

        var actual = collection.GetSetting(key);
        testOutputHelper.WriteLine("actual: {0}", actual);

        Assert.Equal(expectedSetting, actual);
    }

    [Theory]
    [InlineData(@"Data Source=|DataDirectory|\Northwind.dev.sqlite", "Northwind")]
    public void ShouldGetConnectionStringSettings(string expectedConnectionString, string unqualifiedName)
    {
        var configuration = ConfigurationManager
            .OpenExeConfiguration(GetType().Assembly.Location);
        Assert.NotNull(configuration);
        Assert.True(configuration.HasFile);

        var environmentName = configuration
            .AppSettings.Settings
            .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
        var name = configuration
            .ConnectionStrings.ConnectionStrings
            .GetConnectionNameFromEnvironment(unqualifiedName, environmentName);
        testOutputHelper.WriteLine("name: {0}", name);

        var settings = configuration
            .ConnectionStrings.ConnectionStrings
            .GetConnectionStringSettings(name)
            .ToReferenceTypeValueOrThrow();

        var actual = settings.ConnectionString;
        testOutputHelper.WriteLine("actual: {0}", actual);

        Assert.Equal(expectedConnectionString, actual);
    }

    [Theory]
    [InlineData(DeploymentEnvironment.DevelopmentEnvironmentName)]
    public void ShouldGetEnvironmentName(string expectedEnvironmentName)
    {
        testOutputHelper.WriteLine("expected: {0}", expectedEnvironmentName);

        var configuration = ConfigurationManager
            .OpenExeConfiguration(GetType().Assembly.Location);
        Assert.NotNull(configuration);
        Assert.True(configuration.HasFile);

        var actual = configuration.AppSettings.Settings.GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
        testOutputHelper.WriteLine("actual: {0}", actual);

        Assert.Equal(expectedEnvironmentName, actual);
    }

    [Theory]
    [InlineData("dev.setting", "setting")]
    public void ShouldGetKeyWithEnvironmentName(string expectedKey, string unqualifiedKey)
    {
        var configuration = ConfigurationManager
            .OpenExeConfiguration(GetType().Assembly.Location);
        Assert.NotNull(configuration);
        Assert.True(configuration.HasFile);

        var environmentName = configuration.AppSettings.Settings.GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
        var actual = configuration.AppSettings.Settings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
        testOutputHelper.WriteLine("actual: {0}", actual);

        Assert.Equal(expectedKey, actual);
    }

    [Theory]
    [InlineData("the setting for DEV", "setting")]
    public void ShouldGetSetting(string expectedSetting, string unqualifiedKey)
    {
        var configuration = ConfigurationManager
            .OpenExeConfiguration(GetType().Assembly.Location);
        Assert.NotNull(configuration);
        Assert.True(configuration.HasFile);

        var environmentName = configuration.AppSettings.Settings
            .GetEnvironmentName(DeploymentEnvironment.ConfigurationKey, "defaultEnvironmentName");
        var key = configuration.AppSettings.Settings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName);
        testOutputHelper.WriteLine("key: {0}", key);

        var actual = configuration.AppSettings.Settings.GetSetting(key);
        testOutputHelper.WriteLine("actual: {0}", actual);

        Assert.Equal(expectedSetting, actual);
    }
}
