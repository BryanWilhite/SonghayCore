using Songhay.Diagnostics;
using System.Configuration;
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
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException"></exception>
        public static TraceSource GetConfiguredTraceSource(this TraceSources instance, string key)
        {
            if (instance == null) return null;

            var name = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(name)) return null;

            return TraceSources.Instance.GetTraceSource(name);
        }
    }
}
