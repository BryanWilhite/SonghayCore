using System;
using System.Collections;
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
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="set">The <see cref="System.Xml.Linq.XNode"/></param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
        public static string GetXAttributeValue(XNode set, string setQuery, bool throwException)
        {
            return GetXAttributeValue(set, setQuery, throwException, null);
        }

        /// <summary>
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="set">The <see cref="System.Xml.Linq.XNode"/></param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
        /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes</param>
        public static string GetXAttributeValue(XNode set, string setQuery, bool throwException, string defaultValue)
        {
            return XObjectUtility.GetXAttributeValue(set, setQuery, throwException, defaultValue, null);
        }

        /// <summary>
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="set">The <see cref="System.Xml.Linq.XNode"/></param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
        /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes</param>
        /// <param name="resolver">
        /// The <see cref="System.Xml.IXmlNamespaceResolver"/>
        /// to use to resolve prefixes.
        /// </param>
        public static string GetXAttributeValue(XNode set, string setQuery, bool throwException, string defaultValue, IXmlNamespaceResolver resolver)
        {
            if(set == null) return defaultValue;

            var s = defaultValue;

            var a = (resolver == null)
                ? ((IEnumerable)set.XPathEvaluate(setQuery)).OfType<XAttribute>().FirstOrDefault()
                : ((IEnumerable)set.XPathEvaluate(setQuery, resolver)).OfType<XAttribute>().FirstOrDefault();
            if(a != null)
            {
                s = a.Value;
            }
            else if(throwException)
            {
                throw new XmlException(string.Format(CultureInfo.CurrentCulture,"Element at “{0}” was not found.", setQuery));
            }

            return s;
        }

        /// <summary>
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="set">The <see cref="System.Xml.Linq.XNode"/></param>
        /// <param name="setQuery">The XPath <see cref="System.String"/>.</param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes and nodes that do not parse into the specified type.</param>
        /// <param name="defaultValue">Return a boxing <see cref="System.Object"/> for “zero-length” text nodes.</param>
        /// <typeparam name="T">The type to parse from the node value.</typeparam>
        public static object GetXAttributeValueAndParse<T>(XNode set, string setQuery, bool throwException, T defaultValue)
        {
            return XObjectUtility.GetXAttributeValueAndParse<T>(set, setQuery, throwException, defaultValue, null);
        }

        /// <summary>
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <typeparam name="T">The type to parse from the node value.</typeparam>
        /// <param name="set">The <see cref="System.Xml.Linq.XNode"/> set.</param>
        /// <param name="setQuery">The XPath <see cref="System.String"/>.</param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes and nodes that do not parse into the specified type.</param>
        /// <param name="defaultValue">Return a boxing <see cref="System.Object"/> for “zero-length” text nodes.</param>
        /// <param name="resolver">The <see cref="System.Xml.IXmlNamespaceResolver"/>
        /// to use to resolve prefixes.</param>
        /// <returns></returns>
        public static object GetXAttributeValueAndParse<T>(XNode set, string setQuery, bool throwException, T defaultValue, IXmlNamespaceResolver resolver)
        {
            string errMsg = null;

            T stronglyTest = default(T);

            object o = null;

            var s = GetXAttributeValue(set, setQuery, throwException, null, resolver);

            try
            {
                if(string.IsNullOrWhiteSpace(s.Trim()))
                {
                    return defaultValue;
                }
                else if(stronglyTest is bool)
                {
                    o = bool.Parse(s);
                }
                else if(stronglyTest is Byte)
                {
                    o = Byte.Parse(s, CultureInfo.InvariantCulture);
                }
                else if(stronglyTest is DateTime)
                {
                    o = DateTime.Parse(s, CultureInfo.InvariantCulture);
                }
                else if(stronglyTest is Decimal)
                {
                    o = Decimal.Parse(s, CultureInfo.InvariantCulture);
                }
                else if(stronglyTest is Double)
                {
                    o = Double.Parse(s, CultureInfo.InvariantCulture);
                }
                else if(stronglyTest is Int16)
                {
                    o = Int16.Parse(s, CultureInfo.InvariantCulture);
                }
                else if(stronglyTest is Int32)
                {
                    o = Int32.Parse(s, CultureInfo.InvariantCulture);
                }
                else if(stronglyTest is Int64)
                {
                    o = Int64.Parse(s, CultureInfo.InvariantCulture);
                }
                else if(typeof(T).IsAssignableFrom(typeof(string)))
                {
                    o = s;
                }
                else
                {
                    Type t = typeof(T);
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The specified type, “{0},” is not supported.", t.FullName));
                }
            }
            catch(Exception ex)
            {
                Type t = typeof(T);
                errMsg = string.Format(CultureInfo.CurrentCulture, "Parse for “{0}” fails for element in “{1}.” Value to parse: “{2}.” Default Message: “{3}.”",
                    t.FullName, setQuery, (s == null) ? "Null" : s, ex.Message);
                throw new XmlException(errMsg);
            }

            return o;
        }
    }
}
