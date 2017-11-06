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
        /// Builds <see cref="IConfigurationBuilder"/>
        /// with the conventional settings json file.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IConfigurationBuilder WithSettingsJsonFile(this IConfigurationBuilder builder)
        {
            if (builder == null) return null;

            var basePath = Directory.GetCurrentDirectory();
            var settingsFile = "app-settings.json";

            var args = new ProgramArgs(Environment.GetCommandLineArgs());

            var isBasePathRequired = args.HasArg(ProgramArgs.BasePathRequired, requiresValue: false);
            if(args.HasArg(ProgramArgs.BasePath, isBasePathRequired))
            {
                basePath = args.GetArgValue(ProgramArgs.BasePath);
                if (!Directory.Exists(basePath)) throw new ArgumentException($"{basePath} does not exist.");
            }

            if(args.HasArg(ProgramArgs.SettingsFile, requiresValue: false))
            {
                settingsFile = args.GetArgValue(ProgramArgs.SettingsFile);
            }

            return builder
                .SetBasePath(basePath)
                .AddJsonFile(settingsFile, optional: true, reloadOnChange: true);
        }
    }
}
