using System;
using System.Diagnostics;

namespace Songhay.Extensions
{
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
        public static TraceSource EnsureTraceSource(this TraceSource traceSource)
        {
            if (traceSource == null) throw new NullReferenceException("The expected Trace Source is not here.");
            return traceSource;
        }

        /// <summary>
        /// Traces the data.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="data">The data.</param>
        public static void TraceData(this TraceSource traceSource, TraceEventType eventType, int id, params object[] data)
        {
            if (traceSource == null) return;
            traceSource.TraceData(eventType, id, data);
        }

        /// <summary>
        /// Traces the data.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="data">The data.</param>
        public static void TraceData(this TraceSource traceSource, TraceEventType eventType, int id, object data)
        {
            if (traceSource == null) return;
            traceSource.TraceData(eventType, id, data);
        }

        /// <summary>
        /// Traces the error.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void TraceError(this TraceSource traceSource, string format, params object[] args)
        {
            if (traceSource == null) return;
            traceSource.TraceEvent(TraceEventType.Error, 0, format, args);
        }

        /// <summary>
        /// Traces the error.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The message.</param>
        public static void TraceError(this TraceSource traceSource, string message)
        {
            if (traceSource == null) return;
            traceSource.TraceEvent(TraceEventType.Error, 0, message);
        }

        /// <summary>
        /// Traces the error.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="ex">The exception.</param>
        public static void TraceError(this TraceSource traceSource, Exception ex)
        {
            var message = string.Format("{0}\n{1}", ex.Message, ex.StackTrace);
            traceSource.TraceError(message);
        }

        /// <summary>
        /// Trace event type <see cref="TraceEventType.Transfer"/>
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="relatedActivityId">The related activity identifier.</param>
        public static void TraceTransfer(this TraceSource traceSource, int id, string message, Guid relatedActivityId)
        {
            if (traceSource == null) return;
            traceSource.TraceTransfer(id, message, relatedActivityId);
        }

        /// <summary>
        /// Trace event type <see cref="TraceEventType.Warning"/>
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The message.</param>
        public static void TraceWarning(this TraceSource traceSource, string message)
        {
            if (traceSource == null) return;
            traceSource.TraceEvent(TraceEventType.Warning, 0, message);
        }

        /// <summary>
        /// Trace event type <see cref="TraceEventType.Warning"/>
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void TraceWarning(this TraceSource traceSource, string format, params object[] args)
        {
            if (traceSource == null) return;
            traceSource.TraceEvent(TraceEventType.Warning, 0, format, args);
        }

        /// <summary>
        /// Trace event type <see cref="TraceEventType.Verbose"/>
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The message.</param>
        public static void TraceVerbose(this TraceSource traceSource, string message)
        {
            if (traceSource == null) return;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, message);
        }

        /// <summary>
        /// Trace event type <see cref="TraceEventType.Verbose"/>
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void TraceVerbose(this TraceSource traceSource, string format, params object[] args)
        {
            if (traceSource == null) return;
            traceSource.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        /// <summary>
        /// Returns the <see cref="TraceSource"/>
        /// with Switch Level <see cref="SourceLevels.All"/>.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <returns></returns>
        public static TraceSource WithAllSourceLevels(this TraceSource traceSource)
        {
            return traceSource.WithTraceSource(SourceLevels.All);
        }

        /// <summary>
        /// Returns the <see cref="TraceSource"/>
        /// with a single Switch Level.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="level">The level.</param>
        /// <returns></returns>
        public static TraceSource WithTraceSource(this TraceSource traceSource, SourceLevels level)
        {
            if (traceSource == null) return null;
            traceSource.Switch.Level = level;
            return traceSource;
        }

        /// <summary>
        /// Trace event type <see cref="TraceEventType.Information"/>
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// This member has a null check unlike <see cref="TraceSource.TraceInformation(string)"/>
        /// </remarks>
        public static void WriteLine(this TraceSource traceSource, string message)
        {
            if (traceSource == null) return;
            traceSource.TraceInformation(message);
        }

        /// <summary>
        /// Trace event type <see cref="TraceEventType.Information"/>
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <remarks>
        /// This member has a null check unlike <see cref="TraceSource.TraceInformation(string, object[])"/>
        /// </remarks>
        public static void WriteLine(this TraceSource traceSource, string format, params object[] args)
        {
            if (traceSource == null) return;
            traceSource.TraceInformation(format, args);
        }

    }
}
