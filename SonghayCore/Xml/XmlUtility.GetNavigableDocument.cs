using System.Diagnostics.CodeAnalysis;
using System.IO;
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
        /// based on the specified <see cref="System.String"/>.
        /// </summary>
        /// <param name="xmlFragment">
        /// A well-formed XML <see cref="System.String"/>.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
            Justification = "Specific functionality provided by the concrete type may be required.")]
        public static XPathDocument GetNavigableDocument(string xmlFragment)
        {
            XPathDocument d = null;
            using(StringReader reader = new StringReader(xmlFragment))
            {
                d = new XPathDocument(reader);
            }
            return d;
        }

        /// <summary>
        /// Returns an <see cref="System.Xml.XPath.XPathDocument"/>
        /// based on the specified <see cref="System.Xml.XmlNode"/>.
        /// </summary>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <remarks>
        /// Use this member to convert <see cref="System.Xml.XmlDocument"/> sets.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
            Justification = "Specific functionality provided by the concrete type may be required.")]
        public static XPathDocument GetNavigableDocument(IXPathNavigable navigableSet)
        {
            if (navigableSet == null) return null;

            XPathDocument d = null;
            using(StringReader reader = new StringReader(navigableSet.CreateNavigator().OuterXml))
            {
                d = new XPathDocument(reader);
            }
            return d;
        }

        /// <summary>
        /// Returns an <see cref="System.Xml.XPath.XPathDocument"/>
        /// based on the specified <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes",
            Justification = "Specific functionality provided by the concrete type may be required.")]
        public static XPathDocument GetNavigableDocument(Stream stream)
        {
            if(stream == null) return null;
            if(stream.Position != 0) stream.Position = 0;
            XPathDocument d = new XPathDocument(stream);
            return d;
        }
    }
}
