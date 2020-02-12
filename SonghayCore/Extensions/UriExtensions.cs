using System;
using System.IO;
using System.Linq;

#if NETSTANDARD

using System.Collections.Generic;
using System.Net;
using System.Net.Http;

#endif

using System.Threading.Tasks;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Uri"/>
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Determines whether the <see cref="Uri" /> is a file.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <remarks>
        /// Recall that <see cref="Uri.IsFile" /> is another way
        /// of stating that <c>Uri.Schema == Uri.UriSchemeFile</c>
        /// and that <see cref="System.IO"/> members can process URIs.
        /// 
        /// Also note that the only way to truly define a directory
        /// or folder is with a trailing forward/back slash.
        /// </remarks>
        public static bool IsProbablyAFile(this Uri input)
        {
            if (input == null) return false;
            if (input.IsFile) return true;
            return Path.HasExtension(input.OriginalString);
        }

        /// <summary>
        /// Converts the <see cref="Uri" /> into a base URI.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// Returns <see cref="string"/> like: <c>https://MyServer:8080/</c>
        /// </returns>
        public static string ToBaseUri(this Uri input)
        {
            if (input == null) return null;
            var baseLocation = string.Format("{0}/",
                input.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped));
            return baseLocation;
        }

        /// <summary>
        /// Converts the <see cref="Uri" /> into its file name.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// </returns>
        public static string ToFileName(this Uri input)
        {
            return Path.GetFileName(input?.LocalPath);
        }

#if NETSTANDARD

        /// <summary>
        /// Converts the specified <see cref="Uri" />
        /// to its ‘expanded’ version.
        /// </summary>
        /// <param name="expandableUri"></param>
        /// <returns></returns>
        public static async Task<Uri> ToExpandedUriAsync(this Uri expandableUri)
        {
            if (expandableUri == null) throw new ArgumentNullException($"The expected {nameof(expandableUri)} is not here.");

            var message = new HttpRequestMessage(HttpMethod.Get, expandableUri);
            var response = await message.SendAsync();

            if (response.Headers.Location == null)
            {
                return message.RequestUri;
            }

            if (response.IsMovedOrRedirected())
            {
                return response.Headers.Location;
            }

            return await response.Headers.Location.ToExpandedUriAsync();
        }

        /// <summary>
        /// Converts the specified <see cref="Uri" />
        /// to its ‘expanded’ version.
        /// </summary>
        /// <param name="expandableUri"></param>
        /// <returns></returns>
        public static async Task<KeyValuePair<Uri, Uri>> ToExpandedUriPairAsync(this Uri expandableUri)
        {
            var expandedUri = await expandableUri.ToExpandedUriAsync();
            return new KeyValuePair<Uri, Uri>(expandableUri, expandedUri);
        }

#endif

        /// <summary>
        /// Converts the <see cref="Uri"/> into a relative URI from query.
        /// </summary>
        /// <param name="input">The input.</param>
        public static Uri ToRelativeUriFromQuery(this Uri input)
        {
            if (input == null) return null;
            var query = input.OriginalString.Split('?').Last();
            if (string.IsNullOrWhiteSpace(query)) return null;

            return new Uri(query, UriKind.Relative);
        }
    }
}
