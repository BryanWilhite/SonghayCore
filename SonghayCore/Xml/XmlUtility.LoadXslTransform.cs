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
        /// for the transformation of the XSLT file
        /// and the XML file.
        /// </summary>
        /// <param name="xslPath">
        /// outputPath to the XSLT file.
        /// </param>
        /// <param name="xmlPath">
        /// outputPath to the XML file.
        /// </param>
        public static string LoadXslTransform(string xslPath, string xmlPath)
        {
            return XmlUtility.LoadXslTransform(xslPath, new XsltArgumentList(), xmlPath);
        }


        /// <summary>
        /// Returns a <see cref="System.String"/>
        /// for the transformation of the XSLT file
        /// and the XML file.
        /// </summary>
        /// <param name="xslPath">
        /// outputPath to the XSLT file.
        /// </param>
        /// <param name="commandName">
        /// The value for the <code>cmd</code>-parameter convention.
        /// </param>
        /// <param name="xmlFragment">
        /// A well-formed XML <see cref="System.String"/>.
        /// </param>
        /// <remarks>
        /// This convention is used in ASP.NET Web applications.
        /// </remarks>
        public static string LoadXslTransform(string xslPath, string commandName, string xmlFragment)
        {
            XsltArgumentList xslArgs = new XsltArgumentList();
            //CONVENTION: XSL templates use a parameter called “cmd”:
            xslArgs.AddParam("cmd", string.Empty, commandName);

            XslCompiledTransform xslt = new XslCompiledTransform(false);

            string ret = null;
            using(MemoryStream ms = new MemoryStream())
            {
                xslt.Load(xslPath);
                using(StringReader sr = new StringReader(xmlFragment))
                {
                    XmlReader reader = XmlReader.Create(sr);
                    XmlWriter writer = XmlWriter.Create(ms);
                    xslt.Transform(reader, xslArgs, writer, null);
                }

                ret = XmlUtility.GetText(ms);
            }
            return ret;
        }


        /// <summary>
        /// Returns a <see cref="System.String"/>
        /// for the transformation of the XSLT file
        /// and the XML file.
        /// </summary>
        /// <param name="xslPath">
        /// outputPath to the XSLT file.
        /// </param>
        /// <param name="commandName">
        /// The value for the <code>cmd</code>-parameter convention.
        /// </param>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <remarks>
        /// This convention is used in ASP.NET Web applications.
        /// </remarks>
        public static string LoadXslTransform(string xslPath, string commandName, IXPathNavigable navigableSet)
        {
            if(navigableSet == null) throw new ArgumentNullException("navigableSet", "XML set is null");

            XsltArgumentList xslArgs = new XsltArgumentList();
            //CONVENTION: XSL templates use a parameter called “cmd”:
            xslArgs.AddParam("cmd", string.Empty, commandName);

            XslCompiledTransform xslt = new XslCompiledTransform(false);

            string ret = null;
            using(MemoryStream ms = new MemoryStream())
            {
                xslt.Load(xslPath);
                using(StringReader sr = new StringReader(navigableSet.CreateNavigator().OuterXml))
                {
                    XmlReader reader = XmlReader.Create(sr);
                    XmlWriter writer = XmlWriter.Create(ms);
                    xslt.Transform(reader, xslArgs, writer, null);
                }
                ret = XmlUtility.GetText(ms);
            }
            return ret;
        }


        /// <summary>
        /// Returns a <see cref="System.String"/>
        /// for the transformation of the XSLT file
        /// and the XML file.
        /// </summary>
        /// <param name="xslPath">
        /// outputPath to the XSLT file.
        /// </param>
        /// <param name="xslArgs">
        /// The <see cref="System.Xml.Xsl.XsltArgumentList"/>.
        /// </param>
        /// <param name="xmlPath">
        /// outputPath to the XML file.
        /// </param>
        public static string LoadXslTransform(string xslPath, XsltArgumentList xslArgs, string xmlPath)
        {
            XslCompiledTransform xslt = new XslCompiledTransform(false);

            string ret = null;
            using(MemoryStream ms = new MemoryStream())
            {
                xslt.Load(xslPath);
                using(XmlReader reader = XmlReader.Create(xmlPath))
                {
                    XmlWriter writer = XmlWriter.Create(ms);
                    xslt.Transform(reader, xslArgs, writer, null);
                }
                ret = XmlUtility.GetText(ms);
            }
            return ret;
        }
    }
}
