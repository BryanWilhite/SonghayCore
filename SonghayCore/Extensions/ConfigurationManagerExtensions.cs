#if NET452 || NET462

using Songhay.Models;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ConfigurationManager"/>
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
        /// <returns></returns>
        public static string GetConnectionNameFromEnvironment(this ConnectionStringSettingsCollection collection, string unqualifiedKey, string environmentName)
        {
            return collection.GetConnectionNameFromEnvironment(unqualifiedKey, environmentName, DeploymentEnvironment.ConfigurationKeyDelimiter);
        }

        /// <summary>
        /// Gets the connection name from environment.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="unqualifiedKey">The unqualified key.</param>
        /// <param name="environmentName">Name of the environment.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">unqualifiedKey - The expected App Settings key is not here.</exception>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetConnectionNameFromEnvironment(this ConnectionStringSettingsCollection collection, string unqualifiedKey, string environmentName, string delimiter)
        {
            return collection.GetConnectionNameFromEnvironment(unqualifiedKey, environmentName, delimiter, throwConfigurationErrorsException: true);
        }

        /// <summary>
        /// Gets the connection name from environment.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="unqualifiedKey">The unqualified key.</param>
        /// <param name="environmentName">Name of the environment.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
        /// <returns></returns>
        public static string GetConnectionNameFromEnvironment(this ConnectionStringSettingsCollection collection, string unqualifiedKey, string environmentName, bool throwConfigurationErrorsException)
        {
            return collection.GetConnectionNameFromEnvironment(unqualifiedKey, environmentName, DeploymentEnvironment.ConfigurationKeyDelimiter, throwConfigurationErrorsException);
        }

        /// <summary>
        /// Gets the connection name from environment.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="unqualifiedKey">The unqualified key.</param>
        /// <param name="environmentName">Name of the environment.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">unqualifiedKey - The expected App Settings key is not here.</exception>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetConnectionNameFromEnvironment(this ConnectionStringSettingsCollection collection, string unqualifiedKey, string environmentName, string delimiter, bool throwConfigurationErrorsException)
        {
            if (collection == null) return null;
            if (string.IsNullOrWhiteSpace(unqualifiedKey)) throw new ArgumentNullException("unqualifiedKey", "The expected App Settings key is not here.");

            var name1 = string.Concat(environmentName, delimiter, unqualifiedKey);
            var name2 = string.Concat(unqualifiedKey, delimiter, environmentName);

            var containsKey1 = collection.OfType<ConnectionStringSettings>().Any(i => i.Name == name1);
            if (!containsKey1 && !collection.OfType<ConnectionStringSettings>().Any(i => i.Name == name2))
            {
                if (throwConfigurationErrorsException) throw new ConfigurationErrorsException(string.Format("The expected Name, “{0}” or “{1}”, is not here.", name1, name2));
                return null;
            }

            return containsKey1 ? name1 : name2;
        }

        /// <summary>
        /// Gets the connection string settings.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <returns></returns>
        public static ConnectionStringSettings GetConnectionStringSettings(this ConnectionStringSettingsCollection collection, string connectionName)
        {
            return collection.GetConnectionStringSettings(connectionName, throwConfigurationErrorsException: false);
        }

        /// <summary>
        /// Gets the connection string settings.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="connectionName">Name of the connection.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static ConnectionStringSettings GetConnectionStringSettings(this ConnectionStringSettingsCollection collection, string connectionName, bool throwConfigurationErrorsException)
        {
            if (collection == null) return null;
            if (string.IsNullOrWhiteSpace(connectionName)) return null;

            var setting = collection[connectionName];
            if ((setting == null) && throwConfigurationErrorsException)
            {
                var message = string.Format("The expected connection settings, {0}, are not here.", connectionName);
                throw new ConfigurationErrorsException(message);
            }

            return setting;
        }

        /// <summary>
        /// Gets the name of the conventional deployment environment.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public static string GetEnvironmentName(this NameValueCollection settings)
        {
            return settings.GetEnvironmentName(environmentKey: DeploymentEnvironment.ConfigurationKey, defaultEnvironmentName: DeploymentEnvironment.DevelopmentEnvironmentName);
        }

        /// <summary>
        /// Gets the name of the conventional deployment environment.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="environmentKey">The environment key.</param>
        public static string GetEnvironmentName(this NameValueCollection settings, string environmentKey)
        {
            return settings.GetEnvironmentName(environmentKey, defaultEnvironmentName: DeploymentEnvironment.DevelopmentEnvironmentName);
        }

        /// <summary>
        /// Gets the name of the conventional deployment environment.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="environmentKey">The environment key.</param>
        /// <param name="defaultEnvironmentName">Default name of the environment.</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetEnvironmentName(this NameValueCollection settings, string environmentKey, string defaultEnvironmentName)
        {
            return settings.GetEnvironmentName(environmentKey, defaultEnvironmentName, throwConfigurationErrorsException: true);
        }

        /// <summary>
        /// Gets the name of the conventional deployment environment.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="environmentKey">The environment key.</param>
        /// <param name="defaultEnvironmentName">Default name of the environment.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetEnvironmentName(this NameValueCollection settings, string environmentKey, string defaultEnvironmentName, bool throwConfigurationErrorsException)
        {
            if (settings == null) return null;

            var hasKey = settings.AllKeys.Contains(environmentKey);

            if (!hasKey && !string.IsNullOrWhiteSpace(defaultEnvironmentName))
                return defaultEnvironmentName;

            if (!hasKey && string.IsNullOrWhiteSpace(defaultEnvironmentName))
            {
                if (throwConfigurationErrorsException) throw new ConfigurationErrorsException(string.Format("The expected Environment Key, `{0}`, is not here.", environmentKey));
                return null;
            }

            return settings.GetSetting(environmentKey, throwConfigurationErrorsException: true);
        }

        /// <summary>
        /// Gets the name of the key with environment.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="unqualifiedKey">The unqualified key.</param>
        /// <param name="environmentName">Name of the environment.</param>
        /// <returns></returns>
        public static string GetKeyWithEnvironmentName(this NameValueCollection settings, string unqualifiedKey, string environmentName)
        {
            return settings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName, DeploymentEnvironment.ConfigurationKeyDelimiter);
        }

        /// <summary>
        /// Gets the key with environment name.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="unqualifiedKey">The unqualified key.</param>
        /// <param name="environmentName">Name of the environment.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">unqualifiedKey - The expected App Settings key is not here.</exception>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetKeyWithEnvironmentName(this NameValueCollection settings, string unqualifiedKey, string environmentName, string delimiter)
        {
            return settings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName, delimiter, throwConfigurationErrorsException: true);
        }

        /// <summary>
        /// Gets the name of the key with environment.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="unqualifiedKey">The unqualified key.</param>
        /// <param name="environmentName">Name of the environment.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> [throw configuration errors exception].</param>
        /// <returns></returns>
        public static string GetKeyWithEnvironmentName(this NameValueCollection settings, string unqualifiedKey, string environmentName, bool throwConfigurationErrorsException)
        {
            return settings.GetKeyWithEnvironmentName(unqualifiedKey, environmentName, DeploymentEnvironment.ConfigurationKeyDelimiter, throwConfigurationErrorsException);
        }

        /// <summary>
        /// Gets the key with environment name.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="unqualifiedKey">The unqualified key.</param>
        /// <param name="environmentName">Name of the environment.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">unqualifiedKey - The expected App Settings key is not here.</exception>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetKeyWithEnvironmentName(this NameValueCollection settings, string unqualifiedKey, string environmentName, string delimiter, bool throwConfigurationErrorsException)
        {
            if (settings == null) return null;
            if (string.IsNullOrWhiteSpace(unqualifiedKey)) throw new ArgumentNullException("unqualifiedKey", "The expected App Settings key is not here.");

            var key1 = string.Concat(environmentName, delimiter, unqualifiedKey);
            var key2 = string.Concat(unqualifiedKey, delimiter, environmentName);

            var containsKey1 = settings.AllKeys.Contains(key1);
            if (!containsKey1 && !settings.AllKeys.Contains(key2))
            {
                if (throwConfigurationErrorsException) throw new ConfigurationErrorsException(string.Format("The expected Key, “{0}” or “{1}”, is not here.", key1, key2));
                return null;
            }

            return containsKey1 ? key1 : key2;
        }

        /// <summary>
        /// Gets the setting from the current <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="key">The key.</param>
        public static string GetSetting(this NameValueCollection settings, string key)
        {
            return settings.GetSetting(key, throwConfigurationErrorsException: false);
        }

        /// <summary>
        /// Gets the setting from the current <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="key">The key.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetSetting(this NameValueCollection settings, string key, bool throwConfigurationErrorsException)
        {
            if (settings == null) return null;
            if (string.IsNullOrWhiteSpace(key)) return null;

            var setting = settings[key];
            if ((setting == null) && throwConfigurationErrorsException)
            {
                var message = string.Format("The expected setting, {0}, is not here.", key);
                throw new ConfigurationErrorsException(message);
            }

            return setting;
        }

        /// <summary>
        /// Gets the setting from the current <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="key">The key.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> throw configuration errors exception.</param>
        /// <param name="settingModifier">The setting modifier.</param>
        /// <returns></returns>
        public static string GetSetting(this NameValueCollection settings, string key, bool throwConfigurationErrorsException, Func<string, string> settingModifier)
        {
            var setting = settings.GetSetting(key, throwConfigurationErrorsException);
            return (settingModifier == null) ? setting : settingModifier(setting);
        }

        /// <summary>
        /// Converts the external configuration file to the application settings <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="externalConfigurationDoc">The external configuration document.</param>
        /// <returns></returns>
        public static NameValueCollection ToAppSettings(this XDocument externalConfigurationDoc)
        {
            if (externalConfigurationDoc == null) return null;

            var collection = new NameValueCollection();

            externalConfigurationDoc.Root
                    .Element("appSettings")
                    .Elements("add").ForEachInEnumerable(i => collection.Add(i.Attribute("key").Value, i.Attribute("value").Value));

            return collection;
        }

        /// <summary>
        /// Converts the external configuration file to the application settings <see cref="ConnectionStringSettingsCollection"/>.
        /// </summary>
        /// <param name="externalConfigurationDoc">The external configuration document.</param>
        /// <returns></returns>
        public static ConnectionStringSettingsCollection ToConnectionStringSettingsCollection(this XDocument externalConfigurationDoc)
        {
            if (externalConfigurationDoc == null) return null;

            var collection = new ConnectionStringSettingsCollection();

            externalConfigurationDoc.Root
                    .Element("connectionStrings")
                    .Elements("add").ForEachInEnumerable(i =>
                    {
                        var name = i.Attribute("name").Value;
                        var connectionString = i.Attribute("connectionString").Value;
                        var providerName = i.Attribute("providerName").Value;
                        var settings = new ConnectionStringSettings(name, connectionString, providerName);
                        collection.Add(settings);
                    });

            return collection;
        }

        /// <summary>
        /// Returns <see cref="NameValueCollection" />
        /// with the application settings
        /// of the specified external configuration <see cref="XDocument" />.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="externalConfigurationDoc">The external configuration document.</param>
        /// <returns></returns>
        public static NameValueCollection WithAppSettings(this NameValueCollection settings, XDocument externalConfigurationDoc)
        {
            var externalCollection = externalConfigurationDoc.ToAppSettings();
            if (externalCollection == null) return null;

            settings.AllKeys.ForEachInEnumerable(i => externalCollection.Add(i, settings[i]));

            return externalCollection;
        }

        /// <summary>
        /// Returns <see cref="ConnectionStringSettingsCollection" />
        /// with the application settings
        /// of the specified external configuration <see cref="XDocument" />.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="externalConfigurationDoc">The external configuration document.</param>
        /// <returns></returns>
        public static ConnectionStringSettingsCollection WithConnectionStringSettingsCollection(this ConnectionStringSettingsCollection collection, XDocument externalConfigurationDoc)
        {
            var externalCollection = externalConfigurationDoc.ToConnectionStringSettingsCollection();
            if (externalCollection == null) return null;

            collection.OfType<ConnectionStringSettings>().ForEachInEnumerable(i => externalCollection.Add(i));

            return externalCollection;
        }
    }
}

#endif
