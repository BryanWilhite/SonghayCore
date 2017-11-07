using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
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
        /// Gets the <see cref="XNode"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <returns></returns>
        public static XNode GetXNode(XNode set, string setQuery)
        {
            return GetXObject(set, setQuery) as XNode;
        }

        /// <summary>
        /// Gets the <see cref="XNode"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        public static XNode GetXNode(XNode set, string setQuery, bool throwException)
        {
            return GetXObject(set, setQuery, throwException) as XNode;
        }

        /// <summary>
        /// Gets the <see cref="XNode"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static XNode GetXNode(XNode set, string setQuery, bool throwException, IXmlNamespaceResolver resolver)
        {
            return GetXObject(set, setQuery, throwException, resolver) as XNode;
        }

        /// <summary>
        /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <returns></returns>
        public static IEnumerable<XNode> GetXNodes(XNode set, string setQuery)
        {
            var nodes = GetXObjects(set, setQuery);
            return nodes.OfType<XNode>();
        }

        /// <summary>
        /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        public static IEnumerable<XNode> GetXNodes(XNode set, string setQuery, bool throwException)
        {
            var nodes = GetXObjects(set, setQuery, throwException);
            return nodes.OfType<XNode>();
        }

        /// <summary>
        /// Gets <see cref="IEnumerable{XNode}"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        public static IEnumerable<XNode> GetXNodes(XNode set, string setQuery, bool throwException, IXmlNamespaceResolver resolver)
        {
            var nodes = GetXObjects(set, setQuery, throwException, resolver);
            return nodes.OfType<XNode>();
        }
    }
}
