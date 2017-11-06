using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Songhay.Xml
{
    public static partial class XmlUtility
    {
        /// <summary>
        /// Returns a <see cref="System.String"/>
        /// for the transformation of the XSLT document
        /// and the XML document.
        /// </summary>
        /// <param name="xslSet">The source <see cref="System.Xml.XPath.IXPathNavigable"/> XSL set.</param>
        /// <param name="navigableSet">The source <see cref="System.Xml.XPath.IXPathNavigable"/> XML set.</param>
        public static string GetXslString(IXPathNavigable xslSet, IXPathNavigable navigableSet)
        {
            return XmlUtility.GetXslString(xslSet, null, navigableSet, null);
        }

        /// <summary>
        /// Gets the navigable document.
        /// </summary>
        /// <param name="rss">The RSS.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static object GetNavigableDocument(object rss)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/>
        /// for the transformation of the XSLT document
        /// and the XML document.
        /// </summary>
        /// <param name="xslSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> XSL set.
        /// </param>
        /// <param name="xslArgs">
        /// The <see cref="System.Xml.Xsl.XsltArgumentList"/>.
        /// </param>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> XML set.
        /// </param>
        public static string GetXslString(IXPathNavigable xslSet, XsltArgumentList xslArgs, IXPathNavigable navigableSet)
        {
            return XmlUtility.GetXslString(xslSet, xslArgs, navigableSet, null);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/>
        /// for the transformation of the XSLT document
        /// and the XML document.
        /// </summary>
        /// <param name="xslSet">The source <see cref="System.Xml.XPath.IXPathNavigable"/> XSL set.</param>
        /// <param name="xslArgs">The <see cref="System.Xml.Xsl.XsltArgumentList"/>.</param>
        /// <param name="navigableSet">The source <see cref="System.Xml.XPath.IXPathNavigable"/> XML set.</param>
        /// <param name="settings">The settings.</param>
        public static string GetXslString(IXPathNavigable xslSet, XsltArgumentList xslArgs, IXPathNavigable navigableSet, XmlWriterSettings settings)
        {
            if(xslSet == null) throw new ArgumentNullException("xslSet", "XSL set is null");
            if(navigableSet == null) throw new ArgumentNullException("navigableSet", "XML set is null");

            XslCompiledTransform xslt = new XslCompiledTransform(false);
            xslt.Load(xslSet);

            string ret = null;

            using(MemoryStream ms = new MemoryStream())
            {
                using(StringReader sr = new StringReader(navigableSet.CreateNavigator().OuterXml))
                {
                    XmlReader reader = XmlReader.Create(sr);
                    XmlWriter writer = (settings != null) ? XmlWriter.Create(ms, settings) : XmlWriter.Create(ms);
                    xslt.Transform(reader, xslArgs, writer, null);
                }
                ms.Position = 0;

                ret = XmlUtility.GetText(ms);
            }
            return ret;
        }
    }
}
