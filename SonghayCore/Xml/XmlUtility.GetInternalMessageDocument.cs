using System.Diagnostics.CodeAnalysis;
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
        /// Returns an <see cref="System.Xml.XPath.XPathDocument"/>
        /// based on the specified header and lines.
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageLines">Message lines</param>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "Specific functionality provided by the concrete type may be required.")]
        public static XPathDocument GetInternalMessageDocument(string messageHeader, string[] messageLines)
        {
            return GetInternalMessageDocument(messageHeader, string.Empty, messageLines);
        }


        /// <summary>
        /// Returns an <see cref="System.Xml.XPath.XPathDocument"/>
        /// based on the specified header and lines.
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageCode">Message code for errors, exceptions or faults</param>
        /// <param name="messageLines">Message lines</param>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "Specific functionality provided by the concrete type may be required.")]
        public static XPathDocument GetInternalMessageDocument(string messageHeader, string messageCode, string[] messageLines)
        {
            string s = GetInternalMessage(messageHeader, messageCode, messageLines);
            return GetNavigableDocument(s);
        }
    }
}
