using System;
using System.Xml;

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
        /// Write a message to the specified
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageLines">Message lines</param>
        /// <param name="xmlDataWriter">The <see cref="System.Xml.XmlWriter"/></param>
        public static void GetInternalMessage(string messageHeader, string[] messageLines, XmlWriter xmlDataWriter)
        {
            GetInternalMessage(messageHeader, string.Empty, messageLines, xmlDataWriter, false);
        }

        /// <summary>
        /// Write a message to the specified
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageCode">Message code for errors, exceptions or faults</param>
        /// <param name="messageLines">Message lines</param>
        /// <param name="xmlDataWriter">The <see cref="System.Xml.XmlTextWriter"/></param>
        public static void GetInternalMessage(string messageHeader, string messageCode, string[] messageLines, XmlWriter xmlDataWriter)
        {
            GetInternalMessage(messageHeader, messageCode, messageLines, xmlDataWriter, false);
        }

        /// <summary>
        /// Write a message to the specified
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageLines">Message lines</param>
        /// <param name="xmlDataWriter">The <see cref="System.Xml.XmlWriter"/></param>
        /// <param name="isFragment">When <c>false</c> a new document is started.</param>
        public static void GetInternalMessage(string messageHeader, string[] messageLines, XmlWriter xmlDataWriter, bool isFragment)
        {
            GetInternalMessage(messageHeader, string.Empty, messageLines, xmlDataWriter, isFragment);
        }

        /// <summary>
        /// Write a message to the specified
        /// </summary>
        /// <param name="messageHeader">Message header</param>
        /// <param name="messageCode">Message code for errors, exceptions or faults</param>
        /// <param name="messageLines">Message lines</param>
        /// <param name="xmlDataWriter">The <see cref="System.Xml.XmlWriter"/></param>
        /// <param name="isFragment">When <c>false</c> a new document is started.</param>
        public static void GetInternalMessage(string messageHeader, string messageCode, string[] messageLines, XmlWriter xmlDataWriter, bool isFragment)
        {
            if(xmlDataWriter == null) throw new ArgumentNullException("xmlDataWriter", "Argument xmlDataWriter is null.");

            if (!isFragment) xmlDataWriter.WriteStartDocument();
            xmlDataWriter.WriteStartElement("InternalMessage");

            xmlDataWriter.WriteElementString("Header", messageHeader);
            if (!string.IsNullOrWhiteSpace(messageCode)) xmlDataWriter.WriteElementString("Code", messageCode);

            if ((messageLines != null) && (messageLines.Length > 0))
            {
                foreach (string line in messageLines)
                {
                    xmlDataWriter.WriteElementString("Line", line);
                }
            }

            xmlDataWriter.WriteFullEndElement();
            if (!isFragment) xmlDataWriter.WriteEndDocument();
        }
    }
}
