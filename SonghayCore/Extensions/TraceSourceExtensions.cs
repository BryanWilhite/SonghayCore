using System;
using System.Diagnostics;
using System.Text;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="TraceSource"/>
/// </summary>
/// <remarks>
/// Based on Fonlow.Diagnostics by Zijian Huang [https://github.com/zijianhuang/Fonlow.Diagnostics]
/// Also see “Use TraceSource Efficiently” [http://www.codeproject.com/Tips/1071853/Use-TraceSource-Efficiently]
/// </remarks>
public static class TraceSourceExtensions
{
    /// <summary>
    /// Ensures the trace source.
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException">The expected Trace Source is not here.</exception>
    public static TraceSource EnsureTraceSource(this TraceSource? traceSource)
    {
        if (traceSource == null) throw new NullReferenceException("The expected Trace Source is not here.");

        return traceSource;
    }

    /// <summary>
    /// Traces the error.
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="format">The format.</param>
    /// <param name="args">The arguments.</param>
    public static void TraceError(this TraceSource? traceSource, string format, params object[] args) =>
        traceSource?.TraceEvent(TraceEventType.Error, ++_eventId, format, args);

    /// <summary>
    /// Traces the error.
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="message">The message.</param>
    public static void TraceError(this TraceSource? traceSource, string message) =>
        traceSource?.TraceEvent(TraceEventType.Error, ++_eventId, message);

    /// <summary>
    /// Traces the error.
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="ex">The exception.</param>
    public static void TraceError(this TraceSource? traceSource, Exception ex) =>
        traceSource.TraceError(ex, includeStackTrace: false);

    /// <summary>
    /// Traces the error.
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="ex">The exception.</param>
    /// <param name="includeStackTrace">When <c>true</c>, include <see cref="Exception.StackTrace"/> info</param>
    public static void TraceError(this TraceSource? traceSource, Exception? ex, bool includeStackTrace)
    {
        if (traceSource == null) return;
        if (ex == null) return;

        var sb = new StringBuilder(ex.GetType().Name);

        sb.AppendLine($"{nameof(ex.Message)}: {ex.Message}");

        if (includeStackTrace)
        {
            sb.AppendLine($"{nameof(ex.StackTrace)}:");
            sb.AppendLine($"{ex.StackTrace}");
        }

        traceSource.TraceError(sb.ToString());
    }

    /// <summary>
    /// Trace event type <see cref="TraceEventType.Warning"/>
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="message">The message.</param>
    public static void TraceWarning(this TraceSource? traceSource, string message) =>
        traceSource?.TraceEvent(TraceEventType.Warning, ++_eventId, message);

    /// <summary>
    /// Trace event type <see cref="TraceEventType.Warning"/>
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="format">The format.</param>
    /// <param name="args">The arguments.</param>
    public static void TraceWarning(this TraceSource? traceSource, string format, params object[] args) =>
        traceSource?.TraceEvent(TraceEventType.Warning, ++_eventId, format, args);

    /// <summary>
    /// Trace event type <see cref="TraceEventType.Verbose"/>
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="message">The message.</param>
    public static void TraceVerbose(this TraceSource? traceSource, string message) =>
        traceSource?.TraceEvent(TraceEventType.Verbose, ++_eventId, message);

    /// <summary>
    /// Trace event type <see cref="TraceEventType.Verbose"/>
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="format">The format.</param>
    /// <param name="args">The arguments.</param>
    public static void TraceVerbose(this TraceSource? traceSource, string format, params object[] args) =>
        traceSource?.TraceEvent(TraceEventType.Verbose, ++_eventId, format, args);

    /// <summary>
    /// Returns the <see cref="TraceSource"/>
    /// with Switch Level <see cref="SourceLevels.All"/>.
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <returns></returns>
    public static TraceSource? WithSourceLevels(this TraceSource? traceSource) =>
        traceSource?.WithSourceLevels(SourceLevels.All);

    /// <summary>
    /// Returns the <see cref="TraceSource"/>
    /// with the specified <see cref="SourceLevels"/>.
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="levels">The levels.</param>
    /// <returns></returns>
    public static TraceSource? WithSourceLevels(this TraceSource? traceSource, SourceLevels levels)
    {
        if (traceSource == null) return null;

        traceSource.Switch.Level = levels;

        return traceSource;
    }

    /// <summary>
    /// Trace event type <see cref="TraceEventType.Information"/>
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="message">The message.</param>
    /// <remarks>
    /// This member was previously marked with <see cref="ObsoleteAttribute"/>.
    /// For detail, see https://github.com/BryanWilhite/SonghayCore/issues/82#issuecomment-566214635
    /// </remarks>
    public static void WriteLine(this TraceSource? traceSource, string message) =>
        traceSource?.TraceEvent(TraceEventType.Information, ++_eventId, message);

    /// <summary>
    /// Trace event type <see cref="TraceEventType.Information"/>
    /// </summary>
    /// <param name="traceSource">The trace source.</param>
    /// <param name="format">The format.</param>
    /// <param name="args">The arguments.</param>
    /// <remarks>
    /// This member was previously marked with <see cref="ObsoleteAttribute"/>.
    /// For detail, see https://github.com/BryanWilhite/SonghayCore/issues/82#issuecomment-566214635
    /// </remarks>
    public static void WriteLine(this TraceSource? traceSource, string format, params object[] args) =>
        traceSource?.TraceEvent(TraceEventType.Information, ++_eventId, format, args);

    [ThreadStatic] static int _eventId;
}
