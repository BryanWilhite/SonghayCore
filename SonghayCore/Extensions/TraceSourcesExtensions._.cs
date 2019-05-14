using Songhay.Diagnostics;
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
            if (instance == null) return null;
            return instance.GetTraceSourceFromConfiguredName();
        }
    }
}
