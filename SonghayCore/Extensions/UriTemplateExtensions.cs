#if NETSTANDARD

using System;
using System.Linq;
using Tavis.UriTemplates;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="UriTemplate"/>
    /// </summary>
    public static class UriTemplateExtensions
    {
        /// <summary>
        /// Binds the <see cref="UriTemplate"/>
        /// to the specified <c>params</c> by position.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static Uri BindByPosition(this UriTemplate template, params string[] values) => template.BindByPosition(baseUri: null, values: values);

        /// <summary>
        /// Binds the <see cref="UriTemplate" />
        /// to the specified <c>params</c> by position.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="baseUri">The base URI.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">template - The expected URI template is not here.</exception>
        /// <exception cref="FormatException"></exception>
        public static Uri BindByPosition(this UriTemplate template, Uri baseUri, params string[] values)
        {
            if (template == null) throw new ArgumentNullException("template", "The expected URI template is not here.");

            var keys = template.GetParameterNames();
            for (int i = 0; i < keys.Count(); i++)
            {
                template.AddParameter(keys.ElementAt(i), values.ElementAtOrDefault(i));
            }

            var resolved = template.Resolve();
            if (baseUri != null)
            {
                return new UriBuilder(baseUri).WithPath(resolved).Uri;
            }
            else
            {
                var isAbsolute = Uri.IsWellFormedUriString(resolved, UriKind.Absolute);
                var isRelative = Uri.IsWellFormedUriString(resolved, UriKind.Relative);
                if (!isAbsolute && !isRelative) throw new FormatException($"The resolved URI template, {resolved}, is in an unknown format.");
                return isAbsolute ? new Uri(resolved, UriKind.Absolute) : new Uri(resolved, UriKind.Relative);
            }
        }
    }
}

#endif
