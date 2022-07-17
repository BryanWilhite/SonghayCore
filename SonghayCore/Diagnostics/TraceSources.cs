using System.Collections.Concurrent;

namespace Songhay.Diagnostics;

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
    public static string? ConfiguredTraceSourceName
    {
        get => _configuredTraceSourceName;
        set
        {
            _configuredTraceSourceName = value;
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
    TraceSources() => _traceSources = new ConcurrentDictionary<string, TraceSource>();

    /// <summary>
    /// Gets the name of the trace source from configuration.
    /// </summary>
    /// <remarks>
    /// When the trace source name is not configured
    /// then <see cref="DefaultTraceSourceName"/> is used.
    /// </remarks>
    public TraceSource? GetTraceSourceFromConfiguredName() => GetTraceSource(ConfiguredTraceSourceName);

    /// <summary>
    /// Gets the trace source.
    /// </summary>
    /// <param name="name">The name.</param>
    public TraceSource? GetTraceSource(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        if (_traceSources.TryGetValue(name, out TraceSource? r)) return r;

        r = new TraceSource(name);
        _traceSources.TryAdd(name, r);
        return r;
    }

    /// <summary>
    /// Gets the <see cref="TraceSource"/> with the specified name.
    /// </summary>
    /// <param name="name">The name.</param>
    public TraceSource? this[string? name] => GetTraceSource(name);

    /// <summary>
    /// Gets the instance.
    /// </summary>
    public static TraceSources Instance => Nested.NestedInstance;

    readonly ConcurrentDictionary<string, TraceSource> _traceSources;

    static class Nested
    {
        internal static readonly TraceSources NestedInstance = new();
    }

    static string? _configuredTraceSourceName;
}
