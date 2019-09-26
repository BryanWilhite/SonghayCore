using System.Collections.Concurrent;
using System.Diagnostics;

namespace Songhay.Diagnostics
{
    /// <summary>
    /// Singleton wrapper for <see cref="TraceSource"/>
    /// </summary>
    /// <remarks>
    /// Based on Fonlow.Diagnostics by Zijian Huang [https://github.com/zijianhuang/Fonlow.Diagnostics]
    /// Also see “Use TraceSource Efficiently” [http://www.codeproject.com/Tips/1071853/Use-TraceSource-Efficiently]
    /// </remarks>
    public class TraceSources
    {
        /// <summary>
        /// The conventional <see cref="TraceSource"/> name.
        /// </summary>
        public const string DefaultTraceSourceName = "rx-trace";

        /// <summary>
        /// The configured trace source name
        /// </summary>
        public static string ConfiguredTraceSourceName
        {
            get { return configuredTraceSourceName; }
            set
            {
                configuredTraceSourceName = value;
                IsConfiguredTraceSourceNameLoaded = true;
            }
        }

        /// <summary>
        /// The is configured trace source name loaded?
        /// </summary>
        public static bool IsConfiguredTraceSourceNameLoaded { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="TraceSources"/> class from being created.
        /// </summary>
        TraceSources()
        {
            traceSources = new ConcurrentDictionary<string, TraceSource>();
        }

        /// <summary>
        /// Gets the name of the trace source from configuration.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// When the trace source name is not configured
        /// then <see cref="DefaultTraceSourceName"/> is used.
        /// </remarks>
        public TraceSource GetTraceSourceFromConfiguredName()
        {
            return GetTraceSource(ConfiguredTraceSourceName);
        }

        /// <summary>
        /// Gets the trace source.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public TraceSource GetTraceSource(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            if (traceSources.TryGetValue(name, out TraceSource r)) return r;

            r = new TraceSource(name);
            traceSources.TryAdd(name, r);
            return r;
        }

        /// <summary>
        /// Gets the <see cref="TraceSource"/> with the specified name.
        /// </summary>
        /// <value>
        /// The <see cref="TraceSource"/>.
        /// </value>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public TraceSource this[string name]
        {
            get { return GetTraceSource(name); }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static TraceSources Instance { get { return Nested.instance; } }

        ConcurrentDictionary<string, TraceSource> traceSources;

        static class Nested
        {
            static Nested() { }

            internal static readonly TraceSources instance = new TraceSources();
        }

        static string configuredTraceSourceName;
    }
}