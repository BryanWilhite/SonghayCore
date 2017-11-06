using System;
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
        /// An alternative to <see cref="System.Xml.XPath.XPathNavigator.GetAttribute"/>.
        /// </summary>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <param name="setQuery">
        /// The value to be compiled
        /// into an <see cref="System.Xml.XPath.XPathExpression"/>.
        /// </param>
        [Obsolete("Should be replaced with XmlUtility.GetNodeValue.")]
        public static string GetAttributeValue(IXPathNavigable navigableSet, string setQuery)
        {
            XPathNavigator node = GetNavigableNode(navigableSet, setQuery);
            return (node == null) ? null : node.Value;
        }

        /// <summary>
        /// An alternative to <see cref="System.Xml.XPath.XPathNavigator.GetAttribute"/>.
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
        [Obsolete("Should be replaced with XmlUtility.GetNodeValue.")] 
        public static string GetAttributeValue(IXPathNavigable navigableSet, string setQuery, XmlNamespaceManager nsMan)
        {
            XPathNavigator node = GetNavigableNode(navigableSet, setQuery, nsMan);
            return (node == null) ? null : node.Value;
        }
    }
}
