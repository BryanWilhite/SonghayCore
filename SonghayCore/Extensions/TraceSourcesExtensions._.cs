#if NETSTANDARD

using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Models;
using System.Diagnostics;

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
        /// <returns></returns>
        /// <remarks>
        /// For .NET Standard, this member makes sense after lines like these:
        /// <code>
        ///     var configuration = builder.Build();
        ///     TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];
        /// </code>
        ///
        /// This member makes the <c>GetConfiguredTraceSource</c> pattern cross platform.
        /// </remarks>
        public static TraceSource GetConfiguredTraceSource(this TraceSources instance)
        {
            return instance.GetConfiguredTraceSource(configuration: null, key: null);
        }

        /// <summary>
        /// Gets the configured trace source.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        /// <remarks>
        /// For .NET Standard, this member makes sense after lines like these:
        /// <code>
        ///     var configuration = builder.Build();
        ///     TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];
        /// </code>
        ///
        /// This member makes the <c>GetConfiguredTraceSource</c> pattern cross platform.
        /// </remarks>
        public static TraceSource GetConfiguredTraceSource(this TraceSources instance, IConfiguration configuration)
        {
            return instance.GetConfiguredTraceSource(configuration, key: null);
        }

        /// <summary>
        /// Gets the configured trace source.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <remarks>
        /// For .NET Standard, this member makes sense after lines like these:
        /// <code>
        ///     var configuration = builder.Build();
        ///     TraceSources.ConfiguredTraceSourceName = configuration[DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey];
        /// </code>
        ///
        /// This member makes the <c>GetConfiguredTraceSource</c> pattern cross platform.
        /// </remarks>
        public static TraceSource GetConfiguredTraceSource(this TraceSources instance, IConfiguration configuration, string key)
        {
            if (instance == null) return null;

            if (configuration != null)
            {
                if (string.IsNullOrWhiteSpace(key)) key = DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey;
                TraceSources.ConfiguredTraceSourceName = configuration[key];
            }

            return instance.GetTraceSourceFromConfiguredName();
        }

    }
}

#endif
