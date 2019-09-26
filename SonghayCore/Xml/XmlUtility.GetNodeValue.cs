using System;
using System.Globalization;
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
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
        public static object GetNodeValue(IXPathNavigable node, string setQuery, bool throwException)
        {
            return GetNodeValue(node, setQuery, throwException, null);
        }

        /// <summary>
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
        /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes</param>
        public static object GetNodeValue(IXPathNavigable node, string setQuery, bool throwException, object defaultValue)
        {
            return XmlUtility.GetNodeValue(node, setQuery, throwException, defaultValue, null);
        }

        /// <summary>
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/></param>
        /// <param name="setQuery">The <see cref="System.String"/></param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes.</param>
        /// <param name="defaultValue">Return the specified default value for “zero-length” text nodes</param>
        /// <param name="nsMan">
        /// The <see cref="System.Xml.XmlNamespaceManager"/>
        /// to use to resolve prefixes.
        /// </param>
        public static object GetNodeValue(IXPathNavigable node, string setQuery, bool throwException, object defaultValue, XmlNamespaceManager nsMan)
        {
            XPathNavigator n = XmlUtility.GetNavigableNode(node, setQuery, nsMan);
            object p = defaultValue;

            if(n != null) { if(n.Value.Trim().Length > 0) p = n.Value.Trim(); }
            else if(throwException)
            {
                throw new XmlException(string.Format(CultureInfo.CurrentCulture, "Element at “{0}” was not found.", setQuery));
            }

            return p;
        }

        /// <summary>
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/> to query.</param>
        /// <param name="setQuery">The XPath <see cref="System.String"/>.</param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes and nodes that do not parse into the specified type.</param>
        /// <param name="defaultValue">Return a boxing <see cref="System.Object"/> for “zero-length” text nodes.</param>
        /// <typeparam name="T">The type to parse from the node value.</typeparam>
        public static object GetNodeValueAndParse<T>(IXPathNavigable node, string setQuery, bool throwException, T defaultValue)
        {
            return XmlUtility.GetNodeValueAndParse<T>(node, setQuery, throwException, defaultValue, null);
        }

        /// <summary>
        /// Returns an object for parsing
        /// and adding to a list of parameters for data access.
        /// </summary>
        /// <param name="node">The <see cref="System.Xml.XPath.IXPathNavigable"/> to query.</param>
        /// <param name="setQuery">The XPath <see cref="System.String"/>.</param>
        /// <param name="throwException">When <code>true</code>, throw an exception for null nodes and nodes that do not parse into the specified type.</param>
        /// <param name="defaultValue">Return a boxing <see cref="System.Object"/> for “zero-length” text nodes.</param>
        /// <param name="nsMan">
        /// The <see cref="System.Xml.XmlNamespaceManager"/>
        /// to use to resolve prefixes.
        /// </param>
        /// <typeparam name="T">The type to parse from the node value.</typeparam>
        public static object GetNodeValueAndParse<T>(IXPathNavigable node, string setQuery, bool throwException, T defaultValue, XmlNamespaceManager nsMan)
        {
            string errMsg = null;

            T stronglyT = default(T);

            object p = GetNodeValue(node, setQuery, throwException, nsMan);

            try
            {
                if(p == null)
                {
                    p = defaultValue;
                }
                else if(string.IsNullOrWhiteSpace(p.ToString().Trim()))
                {
                    p = defaultValue;
                }
                else if(stronglyT is bool)
                {
                    p = bool.Parse(p.ToString());
                }
                else if(stronglyT is Byte)
                {
                    p = Byte.Parse(p.ToString(), CultureInfo.CurrentCulture);
                }
                else if(stronglyT is DateTime)
                {
                    p = DateTime.Parse(p.ToString(), CultureInfo.CurrentCulture);
                }
                else if(stronglyT is Decimal)
                {
                    p = Decimal.Parse(p.ToString(), CultureInfo.CurrentCulture);
                }
                else if(stronglyT is Double)
                {
                    p = Double.Parse(p.ToString(), CultureInfo.CurrentCulture);
                }
                else if(stronglyT is Int16)
                {
                    p = Int16.Parse(p.ToString(), CultureInfo.CurrentCulture);
                }
                else if(stronglyT is Int32)
                {
                    p = Int32.Parse(p.ToString(), CultureInfo.CurrentCulture);
                }
                else if(stronglyT is Int64)
                {
                    p = Int64.Parse(p.ToString(), CultureInfo.CurrentCulture);
                }
                else if(typeof(T).IsAssignableFrom(typeof(string)))
                {
                    p = p.ToString();
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
                    t.FullName, setQuery, (p == null) ? "Null" : p.ToString(), ex.Message);
                throw new XmlException(errMsg);
            }

            return p;
        }
    }
}
