using Songhay.Models;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
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
        /// Gets the connection string settings.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static ConnectionStringSettings GetConnectionStringSettings(this ConnectionStringSettingsCollection collection, string key)
        {
            return collection.GetConnectionStringSettings(key, throwConfigurationErrorsException: false);
        }

        /// <summary>
        /// Gets the connection string settings.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> [throw configuration errors exception].</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static ConnectionStringSettings GetConnectionStringSettings(this ConnectionStringSettingsCollection collection, string key, bool throwConfigurationErrorsException)
        {
            if (collection == null) return null;
            if (string.IsNullOrEmpty(key)) return null;

            var setting = collection[key];
            if ((setting == null) && throwConfigurationErrorsException)
            {
                var message = string.Format("The expected connection settings, {0}, are not here.", key);
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
            if (settings == null) return null;

            var hasKey = settings.AllKeys.Contains(environmentKey);

            if (!hasKey && !string.IsNullOrEmpty(defaultEnvironmentName))
                return defaultEnvironmentName;

            if (!hasKey && string.IsNullOrEmpty(defaultEnvironmentName))
                throw new ConfigurationErrorsException(string.Format("The expected Environment Key, `{0}`, is not here.", environmentKey));

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
        /// Gets the name of the key with environment.
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
            if (settings == null) return null;
            if (string.IsNullOrEmpty(unqualifiedKey)) throw new ArgumentNullException("unqualifiedKey", "The expected App Settings key is not here.");

            var key1 = string.Concat(environmentName, delimiter, unqualifiedKey);
            var key2 = string.Concat(unqualifiedKey, delimiter, environmentName);

            var containsKey1 = settings.AllKeys.Contains(key1);
            if (!containsKey1 && !settings.AllKeys.Contains(key2))
                throw new ConfigurationErrorsException(string.Format("The expected Key, “{0}” or “{1}”, is not here.", key1, key2));

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
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> [throw configuration errors exception].</param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static string GetSetting(this NameValueCollection settings, string key, bool throwConfigurationErrorsException)
        {
            if (settings == null) return null;
            if (string.IsNullOrEmpty(key)) return null;

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
        /// <param name="throwConfigurationErrorsException">if set to <c>true</c> [throw configuration errors exception].</param>
        /// <param name="settingModifier">The setting modifier.</param>
        /// <returns></returns>
        public static string GetSetting(this NameValueCollection settings, string key, bool throwConfigurationErrorsException, Func<string, string> settingModifier)
        {
            var setting = settings.GetSetting(key, throwConfigurationErrorsException);
            return (settingModifier == null) ? setting : settingModifier(setting);
        }

        /// <summary>
        /// Loads the external configuration file.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="externalConfigurationFile">The external configuration file.</param>
        /// <returns></returns>
        /// <exception cref="System.IO.FileNotFoundException">
        /// The expected external configuration file path is empty.
        /// or
        /// </exception>
        public static XDocument LoadExternalConfigurationFile(this NameValueCollection settings, string externalConfigurationFile)
        {
            if (settings == null) return null;

            if (string.IsNullOrEmpty(externalConfigurationFile))
                throw new FileNotFoundException("The expected external configuration file path is empty.");

            if (!File.Exists(externalConfigurationFile))
            {
                var message = string.Format("The expected external configuration file, {0}, is not here.", externalConfigurationFile);
                throw new FileNotFoundException(message);
            }

            var externalConfigurationDoc = XDocument.Load(externalConfigurationFile);

            return externalConfigurationDoc;
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
        /// Returns <see cref="NameValueCollection"/>
        /// with the application settings
        /// of the specified external configuration file.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="externalConfigurationFile">The external configuration file.</param>
        /// <returns></returns>
        public static NameValueCollection WithAppSettings(this NameValueCollection settings, string externalConfigurationFile)
        {
            var collection = settings
                .LoadExternalConfigurationFile(externalConfigurationFile)
                .ToAppSettings();

            if (collection == null) return null;

            settings.AllKeys.ForEachInEnumerable(i => collection.Add(i, settings[i]));

            return collection;
        }
    }
}
