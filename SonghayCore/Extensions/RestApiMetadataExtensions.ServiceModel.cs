#if NET452 || NET462

using Songhay.Models;
using System;
using System.Configuration;
using System.Linq;

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
        /// <exception cref="System.ArgumentNullException">bindByPositionValues;The expected bind-by-position values are not here.</exception>
        /// <exception cref="ConfigurationErrorsException">The expected REST API metadata URI template key is not here.</exception>
        public static Uri ToUri(this RestApiMetadata meta, string uriTemplateKey, params string[] bindByPositionValues)
        {
            if (meta == null) return null;
            if ((bindByPositionValues == null) || !bindByPositionValues.Any())
                throw new ArgumentNullException("bindByPositionValues", "The expected bind-by-position values are not here.");
            if (!meta.UriTemplates.Keys.Any(i => i == uriTemplateKey))
                throw new ConfigurationErrorsException("The expected REST API metadata URI template key is not here.");

            var uriTemplate = new UriTemplate(meta.UriTemplates[uriTemplateKey]);
            var uri = uriTemplate.BindByPosition(meta.ApiBase, bindByPositionValues);

            return uri;
        }
    }
}

#endif
