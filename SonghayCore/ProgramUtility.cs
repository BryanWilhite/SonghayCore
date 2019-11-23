using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Extensions;
using System;
using System.Diagnostics;

namespace Songhay
{
    /// <summary>
    /// Defines shared routines for Studio programs
    /// </summary>
    public class ProgramUtility
    {
        /// <summary>
        /// Initializes the trace source.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public static TraceSource InitializeTraceSource(TraceListener listener)
        {
            var traceSource = TraceSources
                .Instance
                .GetTraceSourceFromConfiguredName()
                .WithSourceLevels();

            traceSource?.Listeners.Add(listener);

            return traceSource;
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <returns></returns>
        public static IConfigurationRoot LoadConfiguration(string basePath)
        {
            return LoadConfiguration(basePath, passThroughBuilder: null);
        }

        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="passThroughBuilder">The pass builder.</param>
        /// <returns></returns>
        public static IConfigurationRoot LoadConfiguration(string basePath, Func<IConfigurationBuilder, IConfigurationBuilder> passThroughBuilder)
        {

            Console.WriteLine("Loading configuration...");
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(basePath)
                .AddJsonFile("./appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("./app-settings.songhay-system.json", optional: false, reloadOnChange: false)
                ;

            if (passThroughBuilder != null) builder = passThroughBuilder(builder);

            Console.WriteLine("Building configuration...");
            var configuration = builder.Build();

            return configuration;
        }

        /// <summary>
        /// Handles the debug.
        /// </summary>
        public static void HandleDebug()
        {
#if DEBUG
            Console.WriteLine(string.Format("{0}Press any key to continue...", Environment.NewLine));
            Console.ReadKey(false);
#endif
        }
    }
}
