using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Songhay.Models;

namespace Songhay.Extensions;

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
    public static void DownloadToFile(this HttpWebRequest? request, string? path) =>
        request.DownloadToFile(path, null, bypassProxy: true);

    /// <summary>
    /// Downloads to file.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="path">The path.</param>
    /// <param name="proxyLocation">The proxy location.</param>
    /// <param name="bypassProxy">if set to <c>true</c> [bypass proxy].</param>
    public static void DownloadToFile(this HttpWebRequest? request, string? path, Uri? proxyLocation, bool bypassProxy)
    {
        if (request == null) return;
        path.ThrowWhenNullOrWhiteSpace();

        var buffer = new byte[32768];
        var fileName = Path.GetFileName(path);

        request.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");

        var response = request
            .WithProxy(proxyLocation, bypassProxy)
            .ToHttpWebResponse()
            .ToValueOrThrow();

        var stream = response.GetResponseStream();
        try
        {
            using var fs = File.Create(path);

            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, bytesRead);
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
    public static string? DownloadToString(this HttpWebRequest? request) =>
        request.DownloadToString(null, bypassProxy: true);

    /// <summary>
    /// Downloads to string.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="proxyLocation">The proxy location</param>
    /// <param name="bypassProxy">when <c>true</c>, bypass proxy</param>
    public static string? DownloadToString(this HttpWebRequest? request, Uri? proxyLocation, bool bypassProxy)
    {
        if (request == null) return null;

        string? content;
        var response = request
            .WithProxy(proxyLocation, bypassProxy)
            .ToHttpWebResponse()
            .ToValueOrThrow();
        try
        {
            using var sr = new StreamReader(response.GetResponseStream());

            content = sr.ReadToEnd();
        }
        finally
        {
            response.Close();
        }

        return content;
    }

    /// <summary>
    /// Posts the form.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="postData">The post data.</param>
    public static string? PostForm(this HttpWebRequest? request, Hashtable? postData) =>
        request.PostForm(postData, proxyLocation: null, bypassProxy: true);

    /// <summary>
    /// Posts the form.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="postData">The post data.</param>
    /// <param name="proxyLocation">The proxy location.</param>
    /// <param name="bypassProxy">if set to <c>true</c> [bypass proxy].</param>
    public static string? PostForm(this HttpWebRequest? request, Hashtable? postData,
        Uri? proxyLocation, bool bypassProxy)
    {
        if (request == null) return null;
        ArgumentNullException.ThrowIfNull(postData);

        var postParams = GetPostData(postData);

        request
            .WithProxy(proxyLocation, bypassProxy)
            .WithRequestBody(postParams, "POST", MimeTypes.ApplicationFormUrlEncoded);

        var response = request.DownloadToString();

        return response;
    }

    /// <summary>
    /// Posts the XML.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="xmlFragment">The XML fragment.</param>
    /// <returns></returns>
    public static string? PostXml(this HttpWebRequest? request, string? xmlFragment) =>
        request.PostXml(xmlFragment, null, bypassProxy: true);

    /// <summary>
    /// Posts the XML.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="xmlFragment">The XML fragment.</param>
    /// <param name="proxyLocation">The proxy location.</param>
    /// <param name="bypassProxy">if set to <c>true</c> [bypass proxy].</param>
    /// <returns></returns>
    public static string? PostXml(this HttpWebRequest? request, string? xmlFragment, Uri? proxyLocation,
        bool bypassProxy)
    {
        if (request == null) return null;

        request
            .WithProxy(proxyLocation, bypassProxy)
            .WithRequestBody(xmlFragment, "POST", MimeTypes.TextXml);

        var response = request.DownloadToString();

        return response;
    }

    /// <summary>
    /// Converts the <see cref="HttpWebRequest"/> into a HTTP status code.
    /// </summary>
    /// <param name="request">The request.</param>
    public static HttpStatusCode ToHttpStatusCode(this HttpWebRequest? request)
    {
        if (request == null) return HttpStatusCode.Unused;

        HttpStatusCode code;
        try
        {
            using var response = request.ToHttpWebResponse();

            code = response?.StatusCode ?? HttpStatusCode.Unused;
        }
        catch (WebException ex)
        {
            using HttpWebResponse? response = ex.Response as HttpWebResponse;

            code = response?.StatusCode ?? HttpStatusCode.Unused;
        }

        return code;
    }

    /// <summary>
    /// Converts the <see cref="HttpWebRequest"/> into a HTTP web response.
    /// </summary>
    /// <param name="request">The request.</param>
    public static HttpWebResponse? ToHttpWebResponse(this HttpWebRequest? request) =>
        request?.GetResponse() as HttpWebResponse;

    /// <summary>
    /// Returns the <see cref="HttpWebRequest" />
    /// with a <see cref="WebProxy" />.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="proxyLocation">The proxy location.</param>
    /// <param name="bypassProxy">if set to <c>true</c> [bypass proxy].</param>
    /// <returns></returns>
    public static HttpWebRequest? WithProxy(this HttpWebRequest? request, Uri? proxyLocation, bool bypassProxy)
    {
        if (request == null) return null;

        request.Credentials = CredentialCache.DefaultNetworkCredentials;

        if (proxyLocation != null && (!string.IsNullOrWhiteSpace(proxyLocation.AbsoluteUri)))
            request.Proxy = new WebProxy(proxyLocation, bypassProxy);

        return request;
    }

    /// <summary>
    /// Returns <see cref="HttpWebRequest" /> with the request body.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="requestMethod">The request method.</param>
    public static HttpWebRequest? WithRequestBody(this HttpWebRequest? request, string? requestBody,
        string? requestMethod) => request.WithRequestBody(requestBody, requestMethod, contentType: null);

    /// <summary>
    /// Returns <see cref="HttpWebRequest" /> with the request body.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="requestBody">The request body.</param>
    /// <param name="requestMethod">The request method.</param>
    /// <param name="contentType">The request content type.</param>
    /// <returns></returns>
    public static HttpWebRequest? WithRequestBody(this HttpWebRequest? request, string? requestBody,
        string? requestMethod, string? contentType)
    {
        if (request == null) return null;

        requestBody.ThrowWhenNullOrWhiteSpace();
        requestMethod.ThrowWhenNullOrWhiteSpace();

        if (string.IsNullOrWhiteSpace(contentType)) contentType = MimeTypes.TextPlain;

        request.Method = requestMethod;
        request.ContentType = contentType;

        byte[] body = Encoding.UTF8.GetBytes(requestBody);

        using Stream dataStream = request.GetRequestStream();

        dataStream.Write(body, 0, requestBody.Length);

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
