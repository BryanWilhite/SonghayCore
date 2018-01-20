using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using System.Diagnostics;
using System.IO;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="TraceSources"/>
    /// </summary>
    public static partial class TraceSourcesExtensions
    {
        /// <summary>
        /// Gets the configured trace source.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException"></exception>
        /// <remarks>
        /// This member will use <see cref="Models.ProgramArgs.BasePath"/>
        /// or fall back to <see cref="Directory.GetCurrentDirectory()"/>
        /// to find the configured <see cref="TraceSource"/>.
        /// </remarks>
        public static TraceSource GetConfiguredTraceSource(this TraceSources instance, string key)
        {
            if (instance == null) return null;

            var builder = new ConfigurationBuilder().WithSettingsJsonFile();
            var configuration = builder.Build();
            var name = configuration[key];

            if (string.IsNullOrEmpty(name))
            {
                builder = new ConfigurationBuilder().WithSettingsJsonFile(basePath: Directory.GetCurrentDirectory());
                configuration = builder.Build();
                name = configuration[key];
            }

            if (string.IsNullOrEmpty(name)) return null;

            return TraceSources.Instance.GetTraceSource(name);
        }
    }
}
