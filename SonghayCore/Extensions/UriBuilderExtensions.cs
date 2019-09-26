using System;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="UriBuilder"/>.
    /// </summary>
    public static class UriBuilderExtensions
    {
        /// <summary>
        /// Returns <see cref="UriBuilder"/>
        /// with the specified path.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="path">The path.</param>
        public static UriBuilder WithPath(this UriBuilder builder, string path)
        {
            if (builder == null) return null;
            if (string.IsNullOrWhiteSpace(path)) return builder;

            var delimiter = "/";
            var delimiterChar = '/';

            var baseSegments = builder.Uri.Segments
                .Where(i => i != delimiter)
                .Select(i => i.Trim(delimiterChar));
            var pathSegments = path.Split(delimiterChar)
                .Where(i => !string.IsNullOrWhiteSpace(i));
            var combinedSegments = baseSegments.Union(pathSegments);

            builder.Path = string.Join(delimiter, combinedSegments.ToArray());

            return builder;
        }
    }
}
