#if NETSTANDARD

using Microsoft.Extensions.Configuration;
using Songhay.Models;
using System;
using System.IO;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extension of <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class IConfigurationBuilderExtensions
    {
        /// <summary>
        /// The default settings file name
        /// </summary>
        [Obsolete("These extensions clash with .NET Core Configuration defaults; use ProgramArgsExtensions.")]
        public const string defaultSettingsFileName = "app-settings.json";

        /// <summary>
        /// Builds <see cref="IConfigurationBuilder"/>
        /// with the conventional settings json file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [Obsolete("These extensions clash with .NET Core Configuration defaults; use ProgramArgsExtensions.")]
        public static IConfigurationBuilder WithSettingsJsonFile(this IConfigurationBuilder builder)
        {
            return builder.WithSettingsJsonFile(basePath: null, settingsFileName: null);
        }

        /// <summary>
        /// Builds <see cref="IConfigurationBuilder" />
        /// with the conventional settings json file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="basePath">The base path.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [Obsolete("These extensions clash with .NET Core Configuration defaults; use ProgramArgsExtensions.")]
        public static IConfigurationBuilder WithSettingsJsonFile(this IConfigurationBuilder builder, string basePath)
        {
            return builder.WithSettingsJsonFile(basePath: basePath, settingsFileName: null);
        }

        /// <summary>
        /// Builds <see cref="IConfigurationBuilder" />
        /// with the conventional settings json file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="basePath">The base path.</param>
        /// <param name="settingsFileName">Name of the settings file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <remarks>
        /// When <c>basePath</c> is missing, this member group will not call
        /// <see cref="FileConfigurationExtensions.SetBasePath(IConfigurationBuilder, string)"/>
        /// unless the command-line argument <see cref="ProgramArgs.BasePath"/> is set and is valid.
        ///
        /// When <see cref="ProgramArgs.SettingsFile"/> is not specified then <see cref="defaultSettingsFileName"/> will be used.
        /// </remarks>
        [Obsolete("These extensions clash with .NET Core Configuration defaults; use ProgramArgsExtensions.")]
        public static IConfigurationBuilder WithSettingsJsonFile(this IConfigurationBuilder builder, string basePath, string settingsFileName)
        {
            if (builder == null) return null;

            var args = new ProgramArgs(Environment.GetCommandLineArgs());

            if (string.IsNullOrWhiteSpace(settingsFileName)) settingsFileName = defaultSettingsFileName;

            var isBasePathRequired = args.HasArg(ProgramArgs.BasePathRequired, requiresValue: false);
            if (args.HasArg(ProgramArgs.BasePath, isBasePathRequired))
            {
                basePath = args.GetArgValue(ProgramArgs.BasePath);
                if (!Directory.Exists(basePath)) throw new ArgumentException($"{basePath} does not exist.");
                builder.SetBasePath(basePath);

                if (args.HasArg(ProgramArgs.SettingsFile, requiresValue: false))
                {
                    settingsFileName = args.GetArgValue(ProgramArgs.SettingsFile);
                    builder.AddJsonFile(settingsFileName, optional: true, reloadOnChange: true);
                }
            }
            else if(!string.IsNullOrWhiteSpace(basePath))
            {
                if (!Directory.Exists(basePath)) throw new ArgumentException($"{basePath} does not exist.");
                builder.SetBasePath(basePath);
                builder.AddJsonFile(settingsFileName, optional: true, reloadOnChange: true);
            }

            return builder;
        }
    }
}

#endif
