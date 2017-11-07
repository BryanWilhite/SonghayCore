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
    /// <remarks>
    /// These definitions are biased toward
    /// emitting <see cref="System.Xml.XPath.XPathDocument"/> sets.
    /// However, many accept any input implementing the
    /// <see cref="System.Xml.XPath.IXPathNavigable"/> interface.
    /// </remarks>
    public static partial class XObjectUtility
    {
        /// <summary>
        /// Gets the <see cref="XObject"/> from the specified XPath query..
        /// </summary>
        /// <param name="set">The <see cref="System.Xml.Linq.XNode"/> set.</param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        public static XObject GetXObject(XNode set, string setQuery)
        {
            return GetXObject(set, setQuery, throwException: false, resolver: null);
        }

        /// <summary>
        /// Gets the <see cref="XObject"/> from the specified XPath query..
        /// </summary>
        /// <param name="set">The <see cref="System.Xml.Linq.XNode"/> set.</param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
        public static XObject GetXObject(XNode set, string setQuery, bool throwException)
        {
            return GetXObject(set, setQuery, throwException, resolver: null);
        }

        /// <summary>
        /// Gets the XObject.
        /// </summary>
        /// <param name="set">The <see cref="System.Xml.Linq.XNode"/> set.</param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
        /// <param name="resolver">
        /// The <see cref="System.Xml.IXmlNamespaceResolver"/>
        /// to use to resolve prefixes.
        /// </param>
        public static XObject GetXObject(XNode set, string setQuery, bool throwException, IXmlNamespaceResolver resolver)
        {
            var result = GetXObjects(set, setQuery, throwException, resolver);
            return (result == null) ? null : result.FirstOrDefault();
        }

        /// <summary>
        /// Gets <see cref="IEnumerable{XObject}"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <returns></returns>
        public static IEnumerable<XObject> GetXObjects(XNode set, string setQuery)
        {
            return GetXObjects(set, setQuery, throwException: false, resolver: null);
        }

        /// <summary>
        /// Gets <see cref="IEnumerable{XObject}"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        public static IEnumerable<XObject> GetXObjects(XNode set, string setQuery, bool throwException)
        {
            return GetXObjects(set, setQuery, throwException, resolver: null);
        }

        /// <summary>
        /// Gets <see cref="IEnumerable{XObject}"/> from the specified XPath query.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <param name="setQuery">The set query.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        /// <exception cref="System.Xml.XmlException"></exception>
        public static IEnumerable<XObject> GetXObjects(XNode set, string setQuery, bool throwException, IXmlNamespaceResolver resolver)
        {
            if (set == null) return null;

            var result = (resolver == null)
                ? ((IEnumerable)set.XPathEvaluate(setQuery)).OfType<XObject>()
                : ((IEnumerable)set.XPathEvaluate(setQuery, resolver)).OfType<XObject>();
            if (result != null)
            {
                return result;
            }
            else if (throwException)
            {
                throw new XmlException(string.Format(CultureInfo.CurrentCulture, "Element at “{0}” was not found.", setQuery));
            }
            else
            {
                return null;
            }
        }
    }
}
