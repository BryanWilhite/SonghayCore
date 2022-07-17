using System.Configuration;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="Microsoft.Extensions.Configuration.ConfigurationManager"/>
/// </summary>
/// <remarks>
/// Several members in this class depend on <see cref="DeploymentEnvironment"/> constants.
/// </remarks>
public static class ConfigurationManagerExtensions
{
    /// <summary>
    /// Gets the connection name from environment.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="unqualifiedKey">The unqualified key.</param>
    /// <param name="environmentName">Name of the environment.</param>
    public static string? GetConnectionNameFromEnvironment(this ConnectionStringSettingsCollection? collection,
        string? unqualifiedKey, string? environmentName) =>
        collection.GetConnectionNameFromEnvironment(unqualifiedKey, environmentName,
            DeploymentEnvironment.ConfigurationKeyDelimiter);

    /// <summary>
    /// Gets the connection name from environment.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="unqualifiedKey">The unqualified key.</param>
    /// <param name="environmentName">Name of the environment.</param>
    /// <param name="delimiter">The delimiter.</param>
    public static string? GetConnectionNameFromEnvironment(this ConnectionStringSettingsCollection? collection,
        string? unqualifiedKey, string? environmentName, string? delimiter) =>
        collection.GetConnectionNameFromEnvironment(unqualifiedKey, environmentName, delimiter,
            throwConfigurationErrorsException: true);

    /// <summary>
    /// Gets the connection name from environment.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="unqualifiedKey">The unqualified key.</param>
    /// <param name="environmentName">Name of the environment.</param>
    /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
    public static string? GetConnectionNameFromEnvironment(this ConnectionStringSettingsCollection? collection,
        string? unqualifiedKey, string? environmentName, bool throwConfigurationErrorsException)
    {
        return collection.GetConnectionNameFromEnvironment(unqualifiedKey, environmentName,
            DeploymentEnvironment.ConfigurationKeyDelimiter, throwConfigurationErrorsException);
    }

    /// <summary>
    /// Gets the connection name from environment.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="unqualifiedKey">The unqualified key.</param>
    /// <param name="environmentName">Name of the environment.</param>
    /// <param name="delimiter">The delimiter.</param>
    /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
    public static string? GetConnectionNameFromEnvironment(this ConnectionStringSettingsCollection? collection,
        string? unqualifiedKey, string? environmentName, string? delimiter, bool throwConfigurationErrorsException)
    {
        if (collection == null) return null;
        unqualifiedKey.ThrowWhenNullOrWhiteSpace();

        var name1 = string.Concat(environmentName, delimiter, unqualifiedKey);
        var name2 = string.Concat(unqualifiedKey, delimiter, environmentName);

        var containsKey1 = collection.OfType<ConnectionStringSettings>().Any(i => i.Name == name1);
        if (!containsKey1 && collection.OfType<ConnectionStringSettings>().All(i => i.Name != name2))
        {
            if (throwConfigurationErrorsException)
                throw new ConfigurationErrorsException($"The expected Name, “{name1}” or “{name2}”, is not here.");
            return null;
        }

        return containsKey1 ? name1 : name2;
    }

    /// <summary>
    /// Gets the connection string settings.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="connectionName">Name of the connection.</param>
    public static ConnectionStringSettings? GetConnectionStringSettings(
        this ConnectionStringSettingsCollection? collection, string? connectionName) =>
        collection.GetConnectionStringSettings(connectionName, throwConfigurationErrorsException: false);

    /// <summary>
    /// Gets the connection string settings.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="connectionName">Name of the connection.</param>
    /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
    public static ConnectionStringSettings? GetConnectionStringSettings(
        this ConnectionStringSettingsCollection? collection, string? connectionName,
        bool throwConfigurationErrorsException)
    {
        if (collection == null) return null;
        if (string.IsNullOrWhiteSpace(connectionName)) return null;

        var setting = collection[connectionName];

        if (setting != null || !throwConfigurationErrorsException) return setting;

        var message = $"The expected connection settings, {connectionName}, are not here.";
        throw new ConfigurationErrorsException(message);
    }

    /// <summary>
    /// Gets the name of the conventional deployment environment.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public static string? GetEnvironmentName(this KeyValueConfigurationCollection? settings)
    {
        return settings.GetEnvironmentName(environmentKey: DeploymentEnvironment.ConfigurationKey,
            defaultEnvironmentName: DeploymentEnvironment.DevelopmentEnvironmentName);
    }

    /// <summary>
    /// Gets the name of the conventional deployment environment.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="environmentKey">The environment key.</param>
    public static string? GetEnvironmentName(this KeyValueConfigurationCollection? settings, string? environmentKey)
    {
        return settings.GetEnvironmentName(environmentKey,
            defaultEnvironmentName: DeploymentEnvironment.DevelopmentEnvironmentName);
    }

    /// <summary>
    /// Gets the name of the conventional deployment environment.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="environmentKey">The environment key.</param>
    /// <param name="defaultEnvironmentName">Default name of the environment.</param>
    public static string? GetEnvironmentName(this KeyValueConfigurationCollection? settings, string? environmentKey,
        string defaultEnvironmentName)
    {
        return settings.GetEnvironmentName(environmentKey, defaultEnvironmentName,
            throwConfigurationErrorsException: true);
    }

    /// <summary>
    /// Gets the name of the conventional deployment environment.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="environmentKey">The environment key.</param>
    /// <param name="defaultEnvironmentName">Default name of the environment.</param>
    /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
    public static string? GetEnvironmentName(this KeyValueConfigurationCollection? settings, string? environmentKey,
        string defaultEnvironmentName, bool throwConfigurationErrorsException)
    {
        if (settings == null) return null;

        var hasKey = settings.AllKeys.Contains(environmentKey);

        if (!hasKey && !string.IsNullOrWhiteSpace(defaultEnvironmentName))
            return defaultEnvironmentName;

        if (hasKey || !string.IsNullOrWhiteSpace(defaultEnvironmentName))
            return settings.GetSetting(environmentKey, throwConfigurationErrorsException: true);

        if (throwConfigurationErrorsException)
            throw new ConfigurationErrorsException(
                $"The expected Environment Key, `{environmentKey}`, is not here.");

        return null;
    }

    /// <summary>
    /// Gets the name of the key with environment.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="unqualifiedKey">The unqualified key.</param>
    /// <param name="environmentName">Name of the environment.</param>
    public static string? GetKeyWithEnvironmentName(this KeyValueConfigurationCollection? settings,
        string? unqualifiedKey, string? environmentName)
    {
        return settings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName,
            DeploymentEnvironment.ConfigurationKeyDelimiter);
    }

    /// <summary>
    /// Gets the key with environment name.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="unqualifiedKey">The unqualified key.</param>
    /// <param name="environmentName">Name of the environment.</param>
    /// <param name="delimiter">The delimiter.</param>
    public static string? GetKeyWithEnvironmentName(this KeyValueConfigurationCollection? settings,
        string? unqualifiedKey, string? environmentName, string? delimiter)
    {
        return settings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName, delimiter,
            throwConfigurationErrorsException: true);
    }

    /// <summary>
    /// Gets the name of the key with environment.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="unqualifiedKey">The unqualified key.</param>
    /// <param name="environmentName">Name of the environment.</param>
    /// <param name="throwConfigurationErrorsException">if set to <c>true</c> [throw configuration errors exception].</param>
    public static string? GetKeyWithEnvironmentName(this KeyValueConfigurationCollection? settings,
        string? unqualifiedKey, string? environmentName, bool throwConfigurationErrorsException)
    {
        return settings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName,
            DeploymentEnvironment.ConfigurationKeyDelimiter, throwConfigurationErrorsException);
    }

    /// <summary>
    /// Gets the key with environment name.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="unqualifiedKey">The unqualified key.</param>
    /// <param name="environmentName">Name of the environment.</param>
    /// <param name="delimiter">The delimiter.</param>
    /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
    public static string? GetKeyWithEnvironmentName(this KeyValueConfigurationCollection? settings,
        string? unqualifiedKey, string? environmentName, string? delimiter, bool throwConfigurationErrorsException)
    {
        if (settings == null) return null;
        unqualifiedKey.ThrowWhenNullOrWhiteSpace();

        var key1 = string.Concat(environmentName, delimiter, unqualifiedKey);
        var key2 = string.Concat(unqualifiedKey, delimiter, environmentName);

        var containsKey1 = settings.AllKeys.Contains(key1);

        if (containsKey1 || settings.AllKeys.Contains(key2)) return containsKey1 ? key1 : key2;

        if (throwConfigurationErrorsException)
            throw new ConfigurationErrorsException($"The expected Key, “{key1}” or “{key2}”, is not here.");

        return null;
    }

    /// <summary>
    /// Gets the setting from the current <see cref="KeyValueConfigurationCollection"/>.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="key">The key.</param>
    public static string? GetSetting(this KeyValueConfigurationCollection? settings, string? key) =>
        settings.GetSetting(key, throwConfigurationErrorsException: false);

    /// <summary>
    /// Gets the setting from the current <see cref="KeyValueConfigurationCollection"/>.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="key">The key.</param>
    /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
    public static string? GetSetting(this KeyValueConfigurationCollection? settings, string? key,
        bool throwConfigurationErrorsException)
    {
        if (settings == null) return null;
        if (string.IsNullOrWhiteSpace(key)) return null;

        var setting = settings[key];

        if (setting != null || !throwConfigurationErrorsException) return setting?.Value;

        var message = $"The expected setting, {key}, is not here.";
        throw new ConfigurationErrorsException(message);
    }

    /// <summary>
    /// Gets the setting from the current <see cref="KeyValueConfigurationCollection"/>.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="key">The key.</param>
    /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
    /// <param name="settingModifier">The setting modifier.</param>
    public static string? GetSetting(this KeyValueConfigurationCollection? settings, string? key,
        bool throwConfigurationErrorsException, Func<string?, string>? settingModifier)
    {
        var setting = settings.GetSetting(key, throwConfigurationErrorsException);

        return settingModifier == null ? setting : settingModifier(setting);
    }

    /// <summary>
    /// Converts the external configuration file
    /// to the application settings <see cref="KeyValueConfigurationCollection"/>.
    /// </summary>
    /// <param name="externalConfigurationDoc">The external configuration document.</param>
    public static KeyValueConfigurationCollection? ToAppSettings(this XDocument? externalConfigurationDoc)
    {
        if (externalConfigurationDoc == null) return null;

        var collection = new KeyValueConfigurationCollection();

        externalConfigurationDoc.Root?
            .Element("appSettings")?
            .Elements("add").ForEachInEnumerable(i =>
                collection.Add(i.Attribute("key")?.Value, i.Attribute("value")?.Value));

        return collection;
    }

    /// <summary>
    /// Converts the external configuration file
    /// to the application settings <see cref="ConnectionStringSettingsCollection"/>.
    /// </summary>
    /// <param name="externalConfigurationDoc">The external configuration document.</param>
    public static ConnectionStringSettingsCollection? ToConnectionStringSettingsCollection(
        this XDocument? externalConfigurationDoc)
    {
        if (externalConfigurationDoc == null) return null;

        var collection = new ConnectionStringSettingsCollection();

        externalConfigurationDoc.Root?
            .Element("connectionStrings")?
            .Elements("add").ForEachInEnumerable(i =>
            {
                var name = i.Attribute("name")?.Value;
                var connectionString = i.Attribute("connectionString")?.Value;
                var providerName = i.Attribute("providerName")?.Value;
                var settings = new ConnectionStringSettings(name, connectionString, providerName);
                collection.Add(settings);
            });

        return collection;
    }

    /// <summary>
    /// Returns <see cref="KeyValueConfigurationCollection" />
    /// with the application settings
    /// of the specified external configuration <see cref="XDocument" />.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <param name="externalConfigurationDoc">The external configuration document.</param>
    public static KeyValueConfigurationCollection? WithAppSettings(this KeyValueConfigurationCollection? settings,
        XDocument? externalConfigurationDoc)
    {
        var externalCollection = externalConfigurationDoc.ToAppSettings();
        if (externalCollection == null) return null;

        settings?.AllKeys.ForEachInEnumerable(key =>
            externalCollection.Add(key, settings.GetSetting(key)));

        return externalCollection;
    }

    /// <summary>
    /// Returns <see cref="ConnectionStringSettingsCollection" />
    /// with the application settings
    /// of the specified external configuration <see cref="XDocument" />.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <param name="externalConfigurationDoc">The external configuration document.</param>
    public static ConnectionStringSettingsCollection? WithConnectionStringSettingsCollection(
        this ConnectionStringSettingsCollection? collection, XDocument? externalConfigurationDoc)
    {
        var externalCollection = externalConfigurationDoc.ToConnectionStringSettingsCollection();

        if (externalCollection == null) return null;

        collection?.OfType<ConnectionStringSettings>().ForEachInEnumerable(i => externalCollection.Add(i));

        return externalCollection;
    }
}
