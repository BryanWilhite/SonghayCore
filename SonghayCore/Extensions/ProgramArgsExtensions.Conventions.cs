using Songhay.Models;
using System;
using System.IO;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ProgramArgs"/>
    /// </summary>
    public static partial class ProgramArgsExtensions
    {
        /// <summary>
        /// Gets the conventional base-path value.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetBasePathValue(this ProgramArgs args)
        {
            var isBasePathRequired = args.HasArg(ProgramArgs.BasePathRequired, requiresValue: false);
            if (!args.HasArg(ProgramArgs.BasePath, isBasePathRequired)) return null;

            var basePath = args.GetArgValue(ProgramArgs.BasePath);
            if (!Directory.Exists(basePath)) throw new ArgumentException($"{basePath} does not exist.");
            return basePath;
        }

        /// <summary>
        /// Gets the conventional settings file path.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string GetSettingsFilePath(this ProgramArgs args)
        {
            if (!args.HasArg(ProgramArgs.SettingsFile, requiresValue: false)) return null;
            var settingsFileName = args.GetArgValue(ProgramArgs.SettingsFile);
            return settingsFileName;
        }
    }
}
