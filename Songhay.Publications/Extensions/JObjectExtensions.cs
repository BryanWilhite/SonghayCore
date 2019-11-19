using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Songhay.Diagnostics;
using Songhay.Publications.Models;
using Songhay.Extensions;
using System;
using System.Diagnostics;
using System.Linq;

namespace Songhay.Publications.Extensions
{
    /// <summary>
    /// Extensions of <see cref="JObject"/>
    /// </summary>
    public static class JObjectExtensions
    {
        static JObjectExtensions() => traceSource = TraceSources
                .Instance
                .GetTraceSourceFromConfiguredName()
                .WithSourceLevels();

        static readonly TraceSource traceSource;

        /// <summary>
        /// Converts to <see cref="TDomainData" /> from <see cref="JObject" />.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TDomainData">The type of the domain data.</typeparam>
        /// <param name="jObject">The <see cref="JObject" />.</param>
        /// <returns></returns>
        public static TDomainData FromJObject<TInterface, TDomainData>(this JObject jObject)
            where TDomainData : class
            where TInterface : class
        {
            return jObject.FromJObject<TInterface, TDomainData>(settings: null);
        }

        /// <summary>
        /// Converts to <see cref="TDomainData" /> from <see cref="JObject" />.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TDomainData">The type of the domain data.</typeparam>
        /// <param name="jObject">The <see cref="JObject" />.</param>
        /// <param name="settings">The settings.</param>
        /// <returns></returns>
        /// <remarks>
        /// The default <see cref="JsonSerializerSettings" />
        /// from <see cref="GenericWebContext.GetJsonSerializerSettings{TDomainData}" />
        /// assumes <c>TDomainData</c> derives from an Interface.
        /// </remarks>
        public static TDomainData FromJObject<TInterface, TDomainData>(this JObject jObject, JsonSerializerSettings settings)
            where TDomainData : class
            where TInterface : class
        {
            if (jObject == null) return null;
            if (settings == null) settings = new InterfaceContractResolver<TInterface>().ToJsonSerializerSettings();

            var domainData = jObject.ToObject<TDomainData>(JsonSerializer.Create(settings));

            return domainData;
        }

        /// <summary>
        /// Converts the <see cref="JObject"/> to presentation <see cref="Segment"/>.
        /// </summary>
        /// <param name="jObject">The j object.</param>
        /// <returns></returns>
        public static Segment ToPresentation(this JObject jObject)
        {
            if (jObject == null) return null;

            var rootProperty = "presentation";

            traceSource?.TraceVerbose($"Converting {rootProperty} JSON to {nameof(Segment)} with descendants...");

            var isPostedToServer = jObject[rootProperty]["is-posted-to-server"].Value<bool>();
            if (isPostedToServer)
            {
                var postedDate = jObject[rootProperty]["posted-date"].Value<DateTime>();
                traceSource?.TraceVerbose($"This {rootProperty} was already posted on {postedDate}.");
                return null;
            }

            var jO_segment = jObject[rootProperty][nameof(Segment)].ToObject<JObject>();
            var segment = jO_segment.FromJObject<ISegment, Segment>();
            if (segment == null)
            {
                traceSource?.TraceError($"The expected {nameof(Segment)} is not here.");
                return null;
            }

            var clientId = segment.ClientId;
            if (string.IsNullOrEmpty(clientId))
            {
                traceSource?.TraceError("The expected Client ID is not here.");
                return null;
            }

            traceSource?.TraceVerbose($"{nameof(Segment)}: {segment}");
            var validationResults = segment.ToValidationResults();
            if (validationResults.Any())
            {
                traceSource?.TraceError($"{nameof(Segment)} validation error(s)!");
                traceSource?.TraceError(validationResults.ToDisplayString());
                return null;
            }

            var jA_documents = jO_segment.GetJArray(nameof(segment.Documents), throwException: true);
            if (!jA_documents.OfType<JObject>().Any())
            {
                traceSource?.TraceError($"The expected JObject {nameof(Document)} enumeration is not here.");
                return null;
            }

            jA_documents.OfType<JObject>().ForEachInEnumerable(i =>
            {
                var document = i.FromJObject<IDocument, Document>();
                if (document == null)
                {
                    traceSource?.TraceError($"The expected {nameof(Document)} is not here.");
                    return;
                }

                if (document.ClientId != clientId)
                {
                    traceSource?.TraceError($"The expected {nameof(Document)} Client ID is not here.");
                    return;
                }

                traceSource?.TraceVerbose($"{nameof(Document)}: {document}");
                var validationResultsForDocument = document.ToValidationResults();
                if (validationResultsForDocument.Any())
                {
                    traceSource?.TraceError($"{nameof(Document)} validation error(s)!");
                    traceSource?.TraceError(validationResultsForDocument.ToDisplayString());
                    return;
                }

                var jA_fragments = i.GetJArray(nameof(document.Fragments), throwException: false);
                if ((jA_fragments == null) || !jA_fragments.OfType<JObject>().Any())
                {
                    traceSource?.TraceWarning($"The JObject {nameof(Fragment)} enumeration is not here.");
                    segment.Documents.Add(document);
                    return;
                }

                jA_fragments.OfType<JObject>().ForEachInEnumerable(j =>
                {
                    var fragment = j.FromJObject<IFragment, Fragment>();
                    if (fragment == null)
                    {
                        traceSource?.TraceError($"The expected {nameof(Fragment)} is not here.");
                        return;
                    }

                    if (fragment.ClientId != clientId)
                    {
                        traceSource?.TraceError($"The expected {nameof(Fragment)} Client ID is not here.");
                        return;
                    }

                    traceSource?.TraceVerbose($"{nameof(Fragment)}: {fragment}");
                    var validationResultsForFragment = fragment.ToValidationResults();
                    if (validationResultsForFragment.Any())
                    {
                        traceSource?.TraceError($"{nameof(Fragment)} validation error(s)!");
                        traceSource?.TraceError(validationResultsForFragment.ToDisplayString());
                        return;
                    }

                    document.Fragments.Add(fragment);
                });

                segment.Documents.Add(document);
            });

            return segment;
        }
    }
}
