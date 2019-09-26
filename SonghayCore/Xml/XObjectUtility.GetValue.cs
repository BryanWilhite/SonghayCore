using System;
using System.Xml.Linq;

namespace Songhay.Xml
{
    /// <summary>
    /// Static helper members for XML-related routines.
    /// </summary>
    public static partial class XObjectUtility
    {
        /// <summary>
        /// Get the CDATA value from the specified <see cref="XElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="XElement"/></param>
        public static string GetCDataValue(XElement element)
        {
            return GetCDataValue(element?.FirstNode);
        }

        /// <summary>
        /// Get the CDATA value from the specified <see cref="XNode"/>.
        /// </summary>
        /// <param name="node">The <see cref="XNode"/></param>
        public static string GetCDataValue(XNode node)
        {
            var cData = node as XCData;
            return cData?.Value;
        }

        /// <summary>
        /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <returns></returns>
        public static string GetLocalNameXPathQuery(string childElementName)
        {
            return GetLocalNameXPathQuery(namespacePrefixOrUri: null, childElementName: childElementName, childAttributeName: null);
        }

        /// <summary>
        /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
        /// </summary>
        /// <param name="namespacePrefixOrUri">The namespace prefix or URI.</param>
        /// <param name="childElementName">Name of the child element.</param>
        /// <returns></returns>
        public static string GetLocalNameXPathQuery(string namespacePrefixOrUri, string childElementName)
        {
            return GetLocalNameXPathQuery(namespacePrefixOrUri, childElementName, childAttributeName: null);
        }

        /// <summary>
        /// Gets the <see cref="XNode" /> into a <c>local-name()</c>, XPath-predicate query.
        /// </summary>
        /// <param name="namespacePrefixOrUri">The namespace prefix or URI.</param>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="childAttributeName">Name of the child attribute.</param>
        /// <returns></returns>
        /// <remarks>
        /// This routine is useful when namespace-resolving is not desirable or available.
        /// </remarks>
        public static string GetLocalNameXPathQuery(string namespacePrefixOrUri, string childElementName, string childAttributeName)
        {
            if (string.IsNullOrWhiteSpace(childElementName)) return null;

            if (string.IsNullOrWhiteSpace(childAttributeName))
            {
                return string.IsNullOrWhiteSpace(namespacePrefixOrUri) ?
                    string.Format("./*[local-name()='{0}']", childElementName)
                    :
                    string.Format("./*[namespace-uri()='{0}' and local-name()='{1}']", namespacePrefixOrUri, childElementName);
            }
            else
            {
                return string.IsNullOrWhiteSpace(namespacePrefixOrUri) ?
                    string.Format("./*[local-name()='{0}']/@{1}", childElementName, childAttributeName)
                    :
                    string.Format("./*[namespace-uri()='{0}' and local-name()='{1}']/@{2}", namespacePrefixOrUri, childElementName, childAttributeName);
            }
        }

        /// <summary>
        /// Gets the element or attribute value of the specified element.
        /// </summary>
        /// <param name="currentNode">The current element.</param>
        /// <param name="query">The xpath query.</param>
        public static string GetValue(XNode currentNode, string query)
        {
            return GetValue(currentNode, query, true);
        }

        /// <summary>
        /// Gets the element or attribute value of the specified element.
        /// </summary>
        /// <param name="currentNode">The current </param>
        /// <param name="query">The xpath query.</param>
        /// <param name="throwException">if set to <c>true</c> throw exception.</param>
        public static string GetValue(XNode currentNode, string query, bool throwException)
        {
            var node = XObjectUtility.GetXObject(currentNode, query);
            if(node == null)
            {
                if(throwException) throw new ArgumentException("The XPath query returns a null node", "query");
                else return null;
            }

            if(node.NodeType == System.Xml.XmlNodeType.Element)
                return (node as XElement).Value;
            else if(node.NodeType == System.Xml.XmlNodeType.Attribute)
                return (string)XObjectUtility.GetXAttributeValue(currentNode, query, throwException);
            else
                return null;
        }
    }
}
