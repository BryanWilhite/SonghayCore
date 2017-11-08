using Songhay.Xml;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Songhay.Extensions
{

    /// <summary>
    /// Extensions of <see cref="XNode"/>.
    /// </summary>
    public static class XNodeExtensions
    {
        /// <summary>
        /// Gets the inner XML.
        /// </summary>
        /// <param name="node">The node.</param>
        public static string GetInnerXml(this XNode node)
        {
            return node.GetInnerXml(true, ReaderOptions.None);
        }

        /// <summary>
        /// Gets the inner XML.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="stripNamespaces">if set to <c>true</c> then strip namespaces (<c>true</c> by default).</param>
        /// <param name="options">The options (<see cref="ReaderOptions.None"/> by default).</param>
        /// <remarks>
        /// “If performance is important (e.g. lots of XML, parsed frequently), I'd use Daniel's CreateReader method every time.”
        /// [http://stackoverflow.com/questions/3793/best-way-to-get-innerxml-of-an-xelement]
        /// —Luke Sampson
        /// </remarks>
        public static string GetInnerXml(this XNode node, bool stripNamespaces, ReaderOptions options)
        {
            if (node == null) return null;

            var reader = node.CreateReader(options);
            reader.MoveToContent();
            var innerXml = reader.ReadInnerXml();
            if (stripNamespaces) innerXml = XmlUtility.StripNamespaces(innerXml);
            return innerXml;
        }

        /// <summary>
        /// Gets <see cref="IXmlNamespaceResolver"/> from the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public static IXmlNamespaceResolver GetNamespaceResolver(this XNode node)
        {
            if (node == null) return null;

            var navigator = node.CreateNavigator();
            var resolver = XmlUtility.GetNamespaceManager(navigator);

            return resolver;
        }

        /// <summary>
        /// Gets the outer XML.
        /// </summary>
        /// <param name="node">The node.</param>
        public static string GetOuterXml(this XNode node)
        {
            return node.GetOuterXml(true, ReaderOptions.None);
        }

        /// <summary>
        /// Gets the outer XML.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="stripNamespaces">if set to <c>true</c> then strip namespaces (<c>true</c> by default).</param>
        /// <param name="options">The options (<see cref="ReaderOptions.None"/> by default).</param>
        public static string GetOuterXml(this XNode node, bool stripNamespaces, ReaderOptions options)
        {
            if (node == null) return null;

            var reader = node.CreateReader(options);
            reader.MoveToContent();
            var outerXml = reader.ReadOuterXml();
            if (stripNamespaces) outerXml = XmlUtility.StripNamespaces(outerXml);
            return outerXml;
        }
    }
}
