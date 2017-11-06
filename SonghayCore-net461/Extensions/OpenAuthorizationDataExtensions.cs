using System;
using System.Security.Cryptography;
using System.Text;

namespace Songhay.Extensions
{
    using Songhay.Models;

    /// <summary>
    /// Extensions of <see cref="OpenAuthorizationData"/>.
    /// </summary>
    public static class OpenAuthorizationDataExtensions
    {
        /// <summary>
        /// Gets the name of the twitter base URI with screen.
        /// </summary>
        /// <param name="twitterBaseUri">The twitter base URI.</param>
        /// <param name="screenName">Name of the screen.</param>
        public static Uri GetTwitterBaseUriWithScreenName(this Uri twitterBaseUri, string screenName)
        {
            if (twitterBaseUri == null) return null;
            return new Uri(twitterBaseUri.OriginalString + "?screen_name=" + Uri.EscapeDataString(screenName));
        }

        /// <summary>
        /// Gets the name of the twitter base URI with screen.
        /// </summary>
        /// <param name="twitterBaseUri">The twitter base URI.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="count">The count.</param>
        public static Uri GetTwitterBaseUriWithScreenName(this Uri twitterBaseUri, string screenName, int count)
        {
            if (twitterBaseUri == null) return null;
            return new Uri(twitterBaseUri.OriginalString + string.Format("?count={0}&screen_name={1}", count, Uri.EscapeDataString(screenName)));
        }

        /// <summary>
        /// Gets the twitter request header.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="twitterBaseUri">The twitter base URI.</param>
        /// <param name="screenName">Name of the screen.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        public static string GetTwitterRequestHeader(this OpenAuthorizationData data, Uri twitterBaseUri, string screenName, string httpMethod)
        {
            if (data == null) return null;
            if (twitterBaseUri == null) return null;

            var baseFormat =
                "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name={6}";

            var baseString = string.Format(
                baseFormat,
                data.ConsumerKey,
                data.Nonce,
                data.SignatureMethod,
                data.TimeStamp,
                data.Token,
                data.Version,
                Uri.EscapeDataString(screenName));

            baseString = string.Concat(httpMethod.ToUpper(),"&", Uri.EscapeDataString(twitterBaseUri.OriginalString), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(data.ConsumerSecret), "&", Uri.EscapeDataString(data.TokenSecret));

            string signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                signature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            var headerFormat =
                "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                Uri.EscapeDataString(data.Nonce),
                Uri.EscapeDataString(data.SignatureMethod),
                Uri.EscapeDataString(data.TimeStamp),
                Uri.EscapeDataString(data.ConsumerKey),
                Uri.EscapeDataString(data.Token),
                Uri.EscapeDataString(signature),
                Uri.EscapeDataString(data.Version));

            return authHeader;
        }
    }
}
