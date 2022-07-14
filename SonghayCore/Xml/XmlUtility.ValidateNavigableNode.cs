using System.Xml.Schema;

namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Returns an <see cref="XmlSchema"/> based
    /// on the specified navigable set and validation event handler.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="eventHandler">
    /// The <see cref="ValidationEventHandler"/>
    /// with signature MyHandler(object sender, ValidationEventArgs args).
    /// </param>
    public static XmlSchema? GetXmlSchema(IXPathNavigable navigable, ValidationEventHandler eventHandler)
    {
        ArgumentNullException.ThrowIfNull(navigable);
        ArgumentNullException.ThrowIfNull(eventHandler);

        XPathNavigator? navigator = navigable.CreateNavigator();
        if (navigator == null) throw new NullReferenceException("The expected XPath Navigator is not here.");

        using StringReader s = new StringReader(navigator.OuterXml);
        var schema = XmlSchema.Read(s, eventHandler);

        return schema;
    }

    /// <summary>
    /// Loads an <see cref="XmlSchema"/> based
    /// on the specified navigable set and validation event handler.
    /// </summary>
    /// <param name="pathToSchema">
    /// The valid path to an XML Schema file.
    /// </param>
    /// <param name="eventHandler">
    /// The <see cref="ValidationEventHandler"/>
    /// with signature MyHandler(object sender, ValidationEventArgs args).
    /// </param>
    public static XmlSchema? LoadXmlSchema(string? pathToSchema, ValidationEventHandler? eventHandler)
    {
        pathToSchema.ThrowWhenNullOrWhiteSpace();

        if (!File.Exists(pathToSchema)) throw new FileNotFoundException(nameof(pathToSchema));

        ArgumentNullException.ThrowIfNull(eventHandler);

        using XmlTextReader x = new XmlTextReader(pathToSchema);
        var schema = XmlSchema.Read(x, eventHandler);

        return schema;
    }

    /// <summary>
    /// Validates the specified navigable set
    /// with the specified schema and validation event handler.
    /// </summary>
    /// <param name="navigable">
    /// The source <see cref="IXPathNavigable"/> document.
    /// </param>
    /// <param name="schema">
    /// The <see cref="XmlSchema"/>.
    /// </param>
    /// <param name="eventHandler">
    /// The <see cref="ValidationEventHandler"/>
    /// with signature MyHandler(object sender, ValidationEventArgs args).
    /// </param>
    public static void ValidateNavigableNode(IXPathNavigable? navigable, XmlSchema? schema,
        ValidationEventHandler? eventHandler)
    {
        ArgumentNullException.ThrowIfNull(navigable);
        ArgumentNullException.ThrowIfNull(schema);
        ArgumentNullException.ThrowIfNull(eventHandler);

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas.Add(schema);
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += eventHandler;

        XPathNavigator? navigator = navigable.CreateNavigator();
        if (navigator == null) throw new NullReferenceException("The expected XPath Navigator is not here.");

        using var s = new StringReader(navigator.OuterXml);

        XmlReader.Create(s, settings);
    }
}
