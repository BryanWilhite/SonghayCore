using System.Xml;
using System.Xml.XPath;

namespace Songhay.Xml
{
    public static partial class XmlUtility
    {
        /// <summary>
        /// Returns strongly typed output, converting the XML fragment.
        /// </summary>
        /// <typeparam name="TOut">The <see cref="System.Type"/> of output to return (constrained to <c>class</c>).</typeparam>
        /// <param name="xmlFragment">An XML fragment or document as a well-formed <see cref="System.String"/>.</param>
        /// <returns>Returns the specified <see cref="System.Type"/>.</returns>
        public static TOut OutputAs<TOut>(string xmlFragment) where TOut : class
        {
            if(typeof(TOut).Equals(typeof(string)))
            {
                return xmlFragment as TOut;
            }
            else
            {
                XPathDocument d = XmlUtility.GetNavigableDocument(xmlFragment);
                return XmlUtility.OutputAs<TOut>(d);
            }
        }

        /// <summary>
        /// Returns strongly typed output, converting the Navigable document.
        /// </summary>
        /// <typeparam name="TOut">The <see cref="System.Type"/> of output to return (constrained to <c>class</c>).</typeparam>
        /// <param name="navigableDocument">An <see cref="System.Xml.XPath.IXPathNavigable"/>.</param>
        /// <returns>Returns the specified <see cref="System.Type"/>.</returns>
        public static TOut OutputAs<TOut>(IXPathNavigable navigableDocument) where TOut : class
        {
            if(navigableDocument == null) return default(TOut);

            if(typeof(TOut).Equals(typeof(string)))
            {
                return navigableDocument.CreateNavigator().OuterXml as TOut;
            }
            else if(typeof(TOut).Equals(typeof(XPathDocument)))
            {
                return navigableDocument as TOut;
            }
            else if(typeof(TOut).Equals(typeof(XmlDocument)))
            {
                XmlDocument dom = new XmlDocument();
                dom.LoadXml(navigableDocument.ToString());
                return dom as TOut;
            }
            else
            {
                return default(TOut);
            }
        }
    }
}
