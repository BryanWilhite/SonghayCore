using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Songhay.Xml
{
    public static partial class XmlUtility
    {
        /// <summary>
        /// Returns an <see cref="System.Xml.Schema.XmlSchema"/> based
        /// on the specified navigable set and validation event handler.
        /// </summary>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <param name="eventHandler">
        /// The <see cref="System.Xml.Schema.ValidationEventHandler"/>
        /// with signature MyHandler(object sender, ValidationEventArgs args).
        /// </param>
        public static XmlSchema GetXmlSchema(IXPathNavigable navigableSet, ValidationEventHandler eventHandler)
        {
            if(navigableSet == null) throw new ArgumentNullException("navigableSet", "The navigable set is null.");
            if(eventHandler == null) throw new ArgumentNullException("eventHandler", "The validation event handler is null.");

            XPathNavigator navigator = navigableSet.CreateNavigator();

            XmlSchema schema = null;
            using(StringReader s = new StringReader(navigator.OuterXml))
            {
                schema = XmlSchema.Read(s, eventHandler);

            }
            return schema;
        }

        /// <summary>
        /// Loads an <see cref="System.Xml.Schema.XmlSchema"/> based
        /// on the specified navigable set and validation event handler.
        /// </summary>
        /// <param name="pathToSchema">
        /// The valid path to an XML Schema file.
        /// </param>
        /// <param name="eventHandler">
        /// The <see cref="System.Xml.Schema.ValidationEventHandler"/>
        /// with signature MyHandler(object sender, ValidationEventArgs args).
        /// </param>
        public static XmlSchema LoadXmlSchema(string pathToSchema, ValidationEventHandler eventHandler)
        {
            if(!File.Exists(pathToSchema)) throw new ArgumentException("The path to the schema is not valid.", "pathToSchema");
            if(eventHandler == null) throw new ArgumentNullException("eventHandler", "The validation event handler is null.");

            XmlSchema schema = null;
            using(XmlTextReader x = new XmlTextReader(pathToSchema))
            {
                schema = XmlSchema.Read(x, eventHandler);
            }
            return schema;
        }

        /// <summary>
        /// Validates the specified navigable set
        /// with the specified schema and validation event handler.
        /// </summary>
        /// <param name="navigableSet">
        /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> set.
        /// </param>
        /// <param name="schema">
        /// The <see cref="System.Xml.Schema.XmlSchema"/>.
        /// </param>
        /// <param name="eventHandler">
        /// The <see cref="System.Xml.Schema.ValidationEventHandler"/>
        /// with signature MyHandler(object sender, ValidationEventArgs args).
        /// </param>
        public static void ValidateNavigableNode(IXPathNavigable navigableSet, XmlSchema schema, ValidationEventHandler eventHandler)
        {
            if(navigableSet == null) throw new ArgumentNullException("navigableSet", "The navigable set is null.");
            if(schema == null) throw new ArgumentNullException("schema", "The schema is null.");
            if(eventHandler == null) throw new ArgumentNullException("eventHandler", "The validation event handler is null.");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(schema);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(eventHandler);

            XPathNavigator navigator = navigableSet.CreateNavigator();
            using(StringReader s = new StringReader(navigator.OuterXml))
            {
                XmlReader.Create(s, settings);
            }
        }
    }
}
