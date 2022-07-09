using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Songhay.Extensions;

namespace Songhay.Xml;

/// <summary>
/// Static helper members for XML-related routines.
/// </summary>
/// <remarks>
/// These definitions are biased toward
/// emitting <see cref="System.Xml.XPath.XPathDocument"/> documents.
/// However, many accept any input implementing the
/// <see cref="System.Xml.XPath.IXPathNavigable"/> interface.
/// </remarks>
public static partial class XmlUtility
{
    /// <summary>
    /// Serializes and writes to the specified path.
    /// </summary>
    /// <typeparam name="T">The type of the instance.</typeparam>
    /// <param name="instance">The instance.</param>
    /// <param name="path">The path.</param>
    public static void Write<T>(T instance, string path) where T : class
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        if (path == null) throw new ArgumentNullException(nameof(path));

        var serializer = new XmlSerializer(typeof(T));

        XmlWriterSettings settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true,
            IndentChars = "    ",
            NewLineOnAttributes = false,
            OmitXmlDeclaration = false
        };

        using var writer = XmlWriter.Create(path, settings);

        serializer.Serialize(writer, instance);
    }

    /// <summary>
    /// Transfers the data in the Source to the Destination.
    /// </summary>
    /// <param name="readerSource"><see cref="System.Xml.XmlReader"/></param>
    /// <param name="writerDestination"><see cref="System.Xml.XmlWriter"/></param>
    public static void WriteReader(XmlReader readerSource, XmlWriter? writerDestination)
    {
        if (readerSource == null) throw new ArgumentNullException(nameof(readerSource));
        if (writerDestination == null) throw new ArgumentNullException(nameof(writerDestination));

        while (readerSource is {EOF: false}) writerDestination.WriteNode(readerSource, false);
    }

    /// <summary>
    /// Transforms the specified navigable documents
    /// and writes to disk with the specified path.
    /// </summary>
    /// <param name="xmlInput">The specified input.</param>
    /// <param name="navigableSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="outputPath">
    /// The file-system, target path.
    /// </param>
    public static void WriteXslTransform(IXPathNavigable? xmlInput,
        IXPathNavigable? navigableSet, string? outputPath)
    {
        if (xmlInput == null) throw new ArgumentNullException(nameof(xmlInput));
        if (navigableSet == null) throw new ArgumentNullException(nameof(navigableSet));
        if (outputPath == null) throw new ArgumentNullException(nameof(outputPath));

        using var fs = new FileStream(outputPath, FileMode.Create);

        var xslt = new XslCompiledTransform(false);
        xslt.Load(navigableSet);

        using var sr = new StringReader(xmlInput.CreateNavigator().EnsureXPathNavigator().OuterXml);
        XmlReader reader = XmlReader.Create(sr);
        XmlWriter writer = XmlWriter.Create(fs);

        xslt.Transform(reader, null, writer, null);
    }

    /// <summary>
    /// Transforms the specified input and writes to disk.
    /// </summary>
    /// <param name="xmlInput">The specified input.</param>
    /// <param name="navigableSet">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="outputPath">
    /// The file-system, target path.
    /// </param>
    public static void WriteXslTransform(XmlReader? xmlInput,
        IXPathNavigable? navigableSet, string? outputPath)
    {
        if (xmlInput == null) throw new ArgumentNullException(nameof(xmlInput));
        if (navigableSet == null) throw new ArgumentNullException(nameof(navigableSet));
        if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentNullException(nameof(outputPath));

        using var fs = new FileStream(outputPath, FileMode.Create);

        var xslt = new XslCompiledTransform(false);
        xslt.Load(navigableSet);
        XmlWriter writer = XmlWriter.Create(fs);

        xslt.Transform(xmlInput, null, writer, null);
    }
}
