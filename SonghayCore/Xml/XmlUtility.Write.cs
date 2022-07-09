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
    public static void Write<T>(T? instance, string? path) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance);

        path.ThrowWhenNullOrWhiteSpace();

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
    public static void WriteReader(XmlReader? readerSource, XmlWriter? writerDestination)
    {
        ArgumentNullException.ThrowIfNull(readerSource);
        ArgumentNullException.ThrowIfNull(writerDestination);

        while (readerSource is {EOF: false}) writerDestination.WriteNode(readerSource, false);
    }

    /// <summary>
    /// Transforms the specified navigable documents
    /// and writes to disk with the specified path.
    /// </summary>
    /// <param name="xmlInput">The specified input.</param>
    /// <param name="navigableXsl">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="outputPath">
    /// The file-system, target path.
    /// </param>
    public static void WriteXslTransform(IXPathNavigable? xmlInput,
        IXPathNavigable? navigableXsl, string? outputPath)
    {
        ArgumentNullException.ThrowIfNull(xmlInput);
        ArgumentNullException.ThrowIfNull(navigableXsl);
        ArgumentNullException.ThrowIfNull(outputPath);

        using var fs = new FileStream(outputPath, FileMode.Create);

        var xslt = new XslCompiledTransform(false);
        xslt.Load(navigableXsl);

        using var sr = new StringReader(xmlInput.CreateNavigator().ToValueOrThrow().OuterXml);
        XmlReader reader = XmlReader.Create(sr);
        XmlWriter writer = XmlWriter.Create(fs);

        xslt.Transform(reader, null, writer, null);
    }

    /// <summary>
    /// Transforms the specified input and writes to disk.
    /// </summary>
    /// <param name="xmlInput">The specified input.</param>
    /// <param name="navigableXsl">
    /// The source <see cref="System.Xml.XPath.IXPathNavigable"/> document.
    /// </param>
    /// <param name="outputPath">
    /// The file-system, target path.
    /// </param>
    public static void WriteXslTransform(XmlReader? xmlInput,
        IXPathNavigable? navigableXsl, string? outputPath)
    {
        ArgumentNullException.ThrowIfNull(xmlInput);
        ArgumentNullException.ThrowIfNull(navigableXsl);
        outputPath.ThrowWhenNullOrWhiteSpace();

        using var fs = new FileStream(outputPath, FileMode.Create);

        var xslt = new XslCompiledTransform(false);
        xslt.Load(navigableXsl);
        XmlWriter writer = XmlWriter.Create(fs);

        xslt.Transform(xmlInput, null, writer, null);
    }
}
