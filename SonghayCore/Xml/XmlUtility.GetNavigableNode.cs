using System.Xml;
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
        /// Returns an <see cref="System.Xml.XPath.XPathNavigator"/>
        /// based on the setQuery Expression toward the source document.
        /// </summary>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <param name="setQuery">
        /// The value to be compiled
        /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
        /// </param>
        public static XPathNavigator GetNavigableNode(IXPathNavigable navigableSet, string setQuery)
        {
            XPathNavigator node = null;

            if (navigableSet == null) return node;
            XPathNavigator xset = navigableSet.CreateNavigator();
            XPathExpression xpath = XPathExpression.Compile(setQuery);
            node = xset.SelectSingleNode(xpath);

            return node;
        }

        /// <summary>
        /// Returns an <see cref="System.Xml.XPath.XPathNavigator"/>
        /// based on the setQuery Expression toward the source document.
        /// </summary>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <param name="setQuery">
        /// The value to be compiled
        /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
        /// </param>
        /// <param name="nsMan">
        /// The <see cref="System.Xml.XmlNamespaceManager"/>
        /// to use to resolve prefixes.
        /// </param>
        public static XPathNavigator GetNavigableNode(IXPathNavigable navigableSet, string setQuery, XmlNamespaceManager nsMan)
        {
            XPathNavigator node = null;

            if (navigableSet == null) return node;
            XPathNavigator xset = navigableSet.CreateNavigator();
            XPathExpression xpath = XPathExpression.Compile(setQuery, nsMan);
            node = xset.SelectSingleNode(xpath);

            return node;
        }

        /// <summary>
        /// Returns an <see cref="System.Xml.XPath.XPathNodeIterator"/>
        /// based on the setQuery Expression toward the source document.
        /// </summary>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <param name="setQuery">
        /// The value to be compiled
        /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
        /// </param>
        public static XPathNodeIterator GetNavigableNodes(IXPathNavigable navigableSet, string setQuery)
        {
            XPathNodeIterator nodes = null;

            if (navigableSet == null) return nodes;
            XPathNavigator xset = navigableSet.CreateNavigator();

            XPathExpression xpath = XPathExpression.Compile(setQuery);
            nodes = xset.Select(xpath);

            return nodes;
        }

        /// <summary>
        /// Returns an <see cref="System.Xml.XPath.XPathNodeIterator"/>
        /// based on the setQuery Expression toward the source document.
        /// </summary>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <param name="setQuery">
        /// The value to be compiled
        /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
        /// </param>
        /// <param name="nsMan">
        /// The <see cref="System.Xml.XmlNamespaceManager"/>
        /// to use to resolve prefixes.
        /// </param>
        public static XPathNodeIterator GetNavigableNodes(IXPathNavigable navigableSet, string setQuery, XmlNamespaceManager nsMan)
        {
            XPathNodeIterator nodes = null;

            if (navigableSet == null) return nodes;
            XPathNavigator xset = navigableSet.CreateNavigator();

            XPathExpression xpath = XPathExpression.Compile(setQuery, nsMan);
            nodes = xset.Select(xpath);

            return nodes;
        }
    }
}
