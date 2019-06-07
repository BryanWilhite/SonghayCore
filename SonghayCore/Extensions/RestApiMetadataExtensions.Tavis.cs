#if NETSTANDARD

using Songhay.Models;
using System;
using System.Linq;
using Tavis.UriTemplates;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="RestApiMetadata"/>.
    /// </summary>
    public static partial class RestApiMetadataExtensions
    {
        /// <summary>
        /// To the URI.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="uriTemplateKey">The URI template key.</param>
        /// <param name="bindByPositionValues">The bind by position values.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">bindByPositionValues;The expected bind-by-position values are not here.</exception>
        /// <exception cref="FormatException">The expected REST API metadata URI template key is not here.</exception>
        public static Uri ToUri(this RestApiMetadata meta, string uriTemplateKey, params string[] bindByPositionValues)
        {
            if (meta == null) return null;
            if ((bindByPositionValues == null) || !bindByPositionValues.Any())
                throw new ArgumentNullException("bindByPositionValues", "The expected bind-by-position values are not here.");
            if (!meta.UriTemplates.Keys.Any(i => i == uriTemplateKey))
                throw new FormatException("The expected REST API metadata URI template key is not here.");

            var forwardSlash = "/";
            var uriBase = meta.ApiBase.OriginalString.EndsWith(forwardSlash) ?
                string.Concat(meta.ApiBase.OriginalString, meta.UriTemplates[uriTemplateKey])
                :
                string.Concat(meta.ApiBase.OriginalString, forwardSlash, meta.UriTemplates[uriTemplateKey]);

            var uriTemplate = new UriTemplate(uriBase);
            var uri = uriTemplate.BindByPosition(bindByPositionValues);

            return uri;
        }
    }
}

#endif
