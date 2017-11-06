using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace Songhay.Xml
{
    public static partial class XmlUtility
    {
        /// <summary>
        /// Returns a <see cref="System.Xml.XmlNamespaceManager"/>
        /// with respect to the document element of the specified
        /// <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </summary>
        /// <param name="navigableSet">
        /// The <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        public static XmlNamespaceManager GetNamespaceManager(IXPathNavigable navigableSet)
        {
            XmlNamespaceManager nsman = null;

            XPathNavigator root;

            if (navigableSet == null) return nsman;
            XPathNavigator xset = navigableSet.CreateNavigator();

            XPathExpression xpath = XPathExpression.Compile("//*[1]");
            root = xset.SelectSingleNode(xpath);

            nsman = new XmlNamespaceManager(root.NameTable);

            IDictionary<string, string> names = root.GetNamespacesInScope(XmlNamespaceScope.Local);
            foreach (KeyValuePair<string, string> kv in names)
            {
                nsman.AddNamespace(kv.Key, kv.Value);
            }

            return nsman;
        }
    }
}
