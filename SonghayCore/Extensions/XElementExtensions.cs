using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Xml.Linq.XElement"/>.
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Adds the once.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attribute">The attribute.</param>
        public static void AddOnce(this XElement element, XAttribute attribute)
        {
            if (element == null) return;
            if (element.HasElementName(attribute.Name)) return;
            element.Add(attribute);
        }

        /// <summary>
        /// Gets the <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        public static XElement GetElement(this XNode node, XName name)
        {
            var element = node as XElement;
            if ((element != null) && (element.Name == name))
            {
                return element;
            }
            return null;
        }

        /// <summary>
        /// Determines whether the <see cref="System.Xml.Linq.XElement"/>
        /// has the specified <see cref="System.Xml.Linq.XName"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// 	<c>true</c> when the element has the name; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasElementName(this XElement element, XName name)
        {
            if (element == null) return false;
            return (element.Name == name);
        }

        /// <summary>
        /// Determines whether the <see cref="System.Xml.Linq.XNode"/>
        /// has the specified <see cref="System.Xml.Linq.XName"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// 	<c>true</c> when the node has the name; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasElementName(this XNode node, XName name)
        {
            var element = node as XElement;
            return (element != null) && (element.Name == name);
        }

        /// <summary>
        /// Determines whether the specified node is <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node is element; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsElement(this XNode node)
        {
            var element = node as XElement;
            return (element != null);
        }

        /// <summary>
        /// Converts the <see cref="XElement" /> into a attribute value or default.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="defaultValue">The default value.</param>
        public static string ToAttributeValueOrDefault(this XElement element, string attributeName, string defaultValue)
        {
            var s = element.ToAttributeValueOrNull(attributeName);
            if (string.IsNullOrWhiteSpace(s)) return defaultValue;
            return s;
        }

        /// <summary>
        /// Returns the attribute value or null.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        public static string ToAttributeValueOrNull(this XElement element, string attributeName)
        {
            if (element == null) return null;

            string s = null;
            var attr = element.Attribute(attributeName);
            if (attr != null) s = attr.Value;
            if (string.IsNullOrWhiteSpace(s)) return null;
            return s;
        }

        /// <summary>
        /// Converts the <see cref="XElement" /> into a element value or default.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="defaultValue">The default value.</param>
        public static string ToElementValueOrDefault(this XElement element, string defaultValue)
        {
            var s = element.ToElementValueOrNull();
            if (string.IsNullOrWhiteSpace(s)) return defaultValue;
            return s;
        }

        /// <summary>
        /// Converts the <see cref="XElement" /> into a element value or null.
        /// </summary>
        /// <param name="element">The element.</param>
        public static string ToElementValueOrNull(this XElement element)
        {
            string s = null;
            if (element != null) s = element.Value;
            if (string.IsNullOrWhiteSpace(s)) return null;
            return s;
        }

        /// <summary>
        /// Returns the element value or null.
        /// </summary>
        /// <param name="elements">The elements.</param>
        public static string ToElementValueOrNull(this IEnumerable<XElement> elements)
        {
            var element = elements.FirstOrDefault();
            return element.ToElementValueOrNull();
        }

        /// <summary>
        /// Returns the specified <see cref="XElement"/>
        /// without namespace qualifiers on elements and attributes.
        /// </summary>
        /// <param name="element">The element</param>
        /// <remarks>
        /// Based on “Answer: How to remove all namespaces from XML with C#?”
        /// [http://stackoverflow.com/a/7238007/22944?stw=2]
        /// </remarks>
        public static XElement WithoutNamespaces(this XElement element)
        {
            if (element == null) return null;

            #region delegates:

            Func<XNode, XNode> getChildNode = e => (e.NodeType == XmlNodeType.Element) ? (e as XElement).WithoutNamespaces() : e;

            Func<XElement, IEnumerable<XAttribute>> getAttributes = e => (e.HasAttributes) ?
                e.Attributes()
                    .Where(a => !a.IsNamespaceDeclaration)
                    .Select(a => new XAttribute(a.Name.LocalName, a.Value))
                :
                Enumerable.Empty<XAttribute>();

            #endregion

            return new XElement(element.Name.LocalName,
                element.Nodes().Select(getChildNode),
                getAttributes(element));
        }
    }
}
