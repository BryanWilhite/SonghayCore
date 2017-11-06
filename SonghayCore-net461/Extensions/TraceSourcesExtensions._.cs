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
        public static TraceSource GetConfiguredTraceSource(this TraceSources instance)
        {
            var key = DeploymentEnvironment.DefaultTraceSourceNameConfigurationKey;
            return instance.GetConfiguredTraceSource(key);
        }
    }
}
