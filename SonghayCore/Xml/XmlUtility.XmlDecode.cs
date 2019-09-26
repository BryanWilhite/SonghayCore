using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Songhay.Xml
{
    /// <summary>
    /// Static helper members for XML-related routines.
    /// </summary>
    /// <remarks>
    /// These definitions are biased toward
    /// emitting <see cref="System.Xml.XPath.XPathDocument"/> sets.
    /// However, many accept any input implementing the
    /// <see cref="System.Xml.XPath.IXPathNavigable"/> interface.
    /// </remarks>
    public static partial class XmlUtility
    {
        /// <summary>
        /// XMLs the encode.
        /// </summary>
        /// <param name="value">The value.</param>
        public static string XmlEncode(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            return value
              .Replace("<", "&lt;")
              .Replace(">", "&gt;")
              .Replace("\"", "&quot;")
              .Replace("'", "&apos;")
              .Replace("&", "&amp;");
        }

        /// <summary>
        /// XMLs the decode.
        /// </summary>
        /// <param name="value">The value.</param>
        public static string XmlDecode(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            return value
              .Replace("&lt;", "<")
              .Replace("&gt;", ">")
              .Replace("&quot;", "\"")
              .Replace("&apos;", "'")
              .Replace("&amp;", "&");
        }
    }
}
