#if NET452 || NET462

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="HttpWebRequest"/>
    /// </summary>
    public static class HttpWebRequestExtensions
    {
        /// <summary>
        /// Downloads to file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="path">The path.</param>
        public static void DownloadToFile(this HttpWebRequest request, string path)
        {
            request.DownloadToFile(path, null, bypassProxy: true);
        }

        /// <summary>
        /// Downloads to file.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="path">The path.</param>
        /// <param name="proxyLocation">The proxy location.</param>
        /// <param name="bypassProxy">if set to <c>true</c> [bypass proxy].</param>
        public static void DownloadToFile(this HttpWebRequest request, string path, Uri proxyLocation, bool bypassProxy)
        {
            if (request == null) return;

            var buffer = new byte[32768];
            var bytesRead = 0;
            var fileName = Path.GetFileName(path);

            request.Headers.Add("Content-Disposition", string.Format("attachment; filename={0}", fileName));

            var response = request
                .WithProxy(proxyLocation, bypassProxy)
                .ToHttpWebResponse();
            var stream = response.GetResponseStream();
            try
            {
                using (var fs = File.Create(path))
                {
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs.Write(buffer, 0, bytesRead);
                    }
                }

            }
            finally
            {
                stream.Close();
                response.Close();
            }
        }

        /// <summary>
        /// Downloads to string.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string DownloadToString(this HttpWebRequest request)
        {
            return request.DownloadToString(null, bypassProxy: true);
        }

        /// <summary>
        /// Downloads to string.
        /// </summary>
        /// <param name="request">The request.</param>
        public static string DownloadToString(this HttpWebRequest request, Uri proxyLocation, bool bypassProxy)
        {
            if (request == null) return null;

            string responseText = null;
            var response = request
                .WithProxy(proxyLocation, bypassProxy)
                .ToHttpWebResponse();
            try
            {
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    responseText = sr.ReadToEnd();
                }
            }
            finally
            {
                response.Close();
            }

            return responseText;
        }

        /// <summary>
        /// Posts the form.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="resourceIndicator">The resource indicator.</param>
        /// <param name="postData">The post data.</param>
        public static string PostForm(this HttpWebRequest request, Uri resourceIndicator, Hashtable postData)
        {
            return request.PostForm(resourceIndicator, postData, null, bypassProxy: true);
        }

        /// <summary>
        /// Posts the form.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="resourceIndicator">The resource indicator.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="proxyLocation">The proxy location.</param>
        /// <param name="bypassProxy">if set to <c>true</c> [bypass proxy].</param>
        public static string PostForm(this HttpWebRequest request, Uri resourceIndicator, Hashtable postData, Uri proxyLocation, bool bypassProxy)
        {
            if (request == null) return null;

            var postParams = GetPostData(postData);

            request
                .WithProxy(proxyLocation, bypassProxy)
                .WithRequestBody(postParams, "POST")
                .ContentType = "application/x-www-form-urlencoded";

            var response = request.DownloadToString();
            return response;
        }

        /// <summary>
        /// Posts the XML.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="xmlFragment">The XML fragment.</param>
        /// <returns></returns>
        public static string PostXml(this HttpWebRequest request, string xmlFragment)
        {
            return request.PostXml(xmlFragment, null, bypassProxy: true);
        }

        /// <summary>
        /// Posts the XML.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="xmlFragment">The XML fragment.</param>
        /// <param name="proxyLocation">The proxy location.</param>
        /// <param name="bypassProxy">if set to <c>true</c> [bypass proxy].</param>
        /// <returns></returns>
        public static string PostXml(this HttpWebRequest request, string xmlFragment, Uri proxyLocation, bool bypassProxy)
        {
            if (request == null) return null;

            request
                .WithProxy(proxyLocation, bypassProxy)
                .WithRequestBody(xmlFragment, "POST")
                .ContentType = "text/xml";

            var response = request.DownloadToString();
            return response;
        }

        /// <summary>
        /// Converts the <see cref="HttpWebRequest"/> into a HTTP status code.
        /// </summary>
        /// <param name="request">The request.</param>
        public static HttpStatusCode ToHttpStatusCode(this HttpWebRequest request)
        {
            if (request == null) return HttpStatusCode.Unused;

            var code = HttpStatusCode.Unused;
            try
            {
                using (var response = request.ToHttpWebResponse())
                {
                    code = response.StatusCode;
                }
            }
            catch (WebException ex)
            {
                using (var response = (HttpWebResponse)ex.Response)
                {
                    code = response.StatusCode;
                }
            }
            return code;
        }

        /// <summary>
        /// Converts the <see cref="HttpWebRequest"/> into a HTTP web response.
        /// </summary>
        /// <param name="request">The request.</param>
        public static HttpWebResponse ToHttpWebResponse(this HttpWebRequest request)
        {
            if (request == null) return null;
            return (HttpWebResponse)request.GetResponse();
        }

        public static HttpWebRequest WithProxy(this HttpWebRequest request, Uri proxyLocation, bool bypassProxy)
        {
            if (request == null) return null;

            request.Credentials = CredentialCache.DefaultNetworkCredentials;

            if ((proxyLocation != null) && (!string.IsNullOrWhiteSpace(proxyLocation.AbsoluteUri)))
                request.Proxy = new WebProxy(proxyLocation, bypassProxy);

            return request;
        }

        /// <summary>
        /// Returns <see cref="HttpWebRequest" /> with the request body.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="requestBody">The request body.</param>
        /// <param name="requestMethod">The request method.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// requestBody;The expected request body is not here.
        /// or
        /// method;The expected request method is not here.
        /// </exception>
        public static HttpWebRequest WithRequestBody(this HttpWebRequest request, string requestBody, string requestMethod)
        {
            if (request == null) return null;
            if (string.IsNullOrWhiteSpace(requestBody)) throw new ArgumentNullException("requestBody", "The expected request body is not here.");
            if (string.IsNullOrWhiteSpace(requestMethod)) throw new ArgumentNullException("method", "The expected request method is not here.");

            request.Method = requestMethod;

            byte[] body = System.Text.Encoding.UTF8.GetBytes(requestBody);

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(body, 0, requestBody.Length);
            }

            return request;
        }

        static string GetPostData(Hashtable postData)
        {
            var sb = new StringBuilder();
            string s = sb.ToString();

            foreach (DictionaryEntry entry in postData)
            {
                s = (string.IsNullOrWhiteSpace(s))
                    ? string.Format(CultureInfo.InvariantCulture, "{0}={1}", entry.Key, entry.Value)
                    : string.Format(CultureInfo.InvariantCulture, "&{0}={1}", entry.Key, entry.Value);
                sb.Append(s);
            }

            return sb.ToString();
        }
    }
}

#endif
