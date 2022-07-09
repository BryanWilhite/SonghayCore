using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns an <see cref="System.Xml.Schema.XmlSchema"/> based
    /// on the specified navigable set and validation event handler.
    /// </summary>
    /// <param name="navigableSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="eventHandler">
    /// The <see cref="System.Xml.Schema.ValidationEventHandler"/>
    /// with signature MyHandler(object sender, ValidationEventArgs args).
    /// </param>
    public static XmlSchema? GetXmlSchema(IXPathNavigable navigableSet, ValidationEventHandler eventHandler)
    {
        if (navigableSet == null) throw new ArgumentNullException(nameof(navigableSet));
        if (eventHandler == null) throw new ArgumentNullException(nameof(eventHandler));

        XPathNavigator? navigator = navigableSet.CreateNavigator();
        if (navigator == null) throw new NullReferenceException("The expected XPath Navigator is not here.");

        using StringReader s = new StringReader(navigator.OuterXml);
        var schema = XmlSchema.Read(s, eventHandler);

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
    public static XmlSchema? LoadXmlSchema(string? pathToSchema, ValidationEventHandler? eventHandler)
    {
        if (string.IsNullOrWhiteSpace(pathToSchema)) throw new ArgumentNullException(nameof(pathToSchema));
        if (!File.Exists(pathToSchema)) throw new FileNotFoundException(nameof(pathToSchema));
        if (eventHandler == null) throw new ArgumentNullException(nameof(eventHandler));

        using XmlTextReader x = new XmlTextReader(pathToSchema);
        var schema = XmlSchema.Read(x, eventHandler);

        return schema;
    }

    /// <summary>
    /// Validates the specified navigable set
    /// with the specified schema and validation event handler.
    /// </summary>
    /// <param name="navigableSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="schema">
    /// The <see cref="System.Xml.Schema.XmlSchema"/>.
    /// </param>
    /// <param name="eventHandler">
    /// The <see cref="System.Xml.Schema.ValidationEventHandler"/>
    /// with signature MyHandler(object sender, ValidationEventArgs args).
    /// </param>
    public static void ValidateNavigableNode(IXPathNavigable? navigableSet, XmlSchema? schema,
        ValidationEventHandler? eventHandler)
    {
        if (navigableSet == null) throw new ArgumentNullException(nameof(navigableSet));
        if (schema == null) throw new ArgumentNullException(nameof(schema));
        if (eventHandler == null) throw new ArgumentNullException(nameof(eventHandler));

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas.Add(schema);
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += eventHandler;

        XPathNavigator? navigator = navigableSet.CreateNavigator();
        if (navigator == null) throw new NullReferenceException("The expected XPath Navigator is not here.");

        using var s = new StringReader(navigator.OuterXml);

        XmlReader.Create(s, settings);
    }
}
