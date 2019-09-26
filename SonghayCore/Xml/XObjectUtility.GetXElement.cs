using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Songhay.Xml
{

    /// <summary>
    /// Static helper members for XML-related routines.
    /// </summary>
    public static partial class XObjectUtility
    {
        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        /// <param name="innerXml">The inner XML.</param>
        public static XElement GetXElement(string rootElement, object innerXml)
        {
            if(string.IsNullOrWhiteSpace(rootElement)) throw new ArgumentNullException("rootElement", "The expected root element name is not here.");
            return XElement.Parse(string.Format("<{0}>{1}</{0}>", rootElement, innerXml));
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="pathToElement">The XPath to element.</param>
        /// <returns></returns>
        public static XElement GetXElement(XNode root, string pathToElement)
        {
            if(root == null) throw new ArgumentNullException("root", "The root element to query is null.");

            return root.XPathSelectElement(pathToElement);
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <param name="currentElement">The current element.</param>
        /// <param name="query">The xpath query.</param>
        public static IEnumerable<XElement> GetXElements(XNode currentElement, string query)
        {
            var nodes = XObjectUtility.GetXNodes(currentElement, query);
            return nodes.OfType<XElement>();
        }
    }
}
