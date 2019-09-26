using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

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
        /// Returns an XML <see cref="System.String"/>
        /// based on the specified header and lines.
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        public static string GetInternalMessage(string messageHeader)
        {
            return GetInternalMessage(messageHeader, string.Empty, new string[] { });
        }

        /// <summary>
        /// Returns an XML <see cref="System.String"/>
        /// based on the specified header and lines.
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageLines">Message lines</param>
        public static string GetInternalMessage(string messageHeader, string[] messageLines)
        {
            return GetInternalMessage(messageHeader, string.Empty, messageLines);
        }

        /// <summary>
        /// Returns an XML <see cref="System.String"/>
        /// based on the specified header and lines.
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageLines">Message lines</param>
        public static string GetInternalMessage(string messageHeader, ReadOnlyCollection<string> messageLines)
        {
            return GetInternalMessage(messageHeader, string.Empty, messageLines);
        }

        /// <summary>
        /// Returns an XML <see cref="System.String"/>
        /// based on the specified header and lines.
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageCode">Message code for errors, exceptions or faults</param>
        /// <param name="messageLines">Message lines</param>
        public static string GetInternalMessage(string messageHeader, string messageCode, string[] messageLines)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<InternalMessage>");

            sb.AppendFormat("<Header>{0}</Header>\n", messageHeader);
            if (!string.IsNullOrWhiteSpace(messageCode)) sb.AppendFormat("<Code>{0}</Code>\n", messageCode);

            if ((messageLines != null) && (messageLines.Length > 0))
            {
                foreach (string line in messageLines)
                {
                    sb.AppendFormat("<Line>{0}</Line>\n", line);
                }

            }
            sb.AppendLine("</InternalMessage>");

            return sb.ToString();
        }

        /// <summary>
        /// Returns an XML <see cref="System.String"/>
        /// based on the specified header and lines.
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageCode">Message code for errors, exceptions or faults</param>
        /// <param name="messageLines">Message lines</param>
        public static string GetInternalMessage(string messageHeader, string messageCode, ReadOnlyCollection<string> messageLines)
        {
            string s = "<InternalMessage>\n";

            s += string.Format(CultureInfo.InvariantCulture, "<Header>{0}</Header>\n", messageHeader);
            if (!string.IsNullOrWhiteSpace(messageCode)) s += string.Format(CultureInfo.InvariantCulture, "<Code>{0}</Code>\n", messageCode);

            if ((messageLines != null) && (messageLines.Count > 0))
            {
                foreach (string line in messageLines)
                {
                    s += string.Format(CultureInfo.InvariantCulture, "<Line>{0}</Line>\n", line);
                }

            }
            return s += "</InternalMessage>\n";
        }
    }
}
