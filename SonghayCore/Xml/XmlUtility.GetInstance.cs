using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// Deserializes based on the specified XML file.
    /// </summary>
    /// <typeparam name="T">
    /// The specified type to deserialize.
    /// </typeparam>
    /// <param name="xmlPath">the XML file path</param>
    public static T? GetInstance<T>(string? xmlPath) where T : class
    {
        var serializer = new XmlSerializer(typeof(T));

        using XmlReader reader = XmlReader.Create(xmlPath!);
        T? instance = serializer.Deserialize(reader) as T;

        return instance;
    }

    /// <summary>
    /// Deserializes based on the specified raw XML.
    /// </summary>
    /// <typeparam name="T">
    /// The specified type to deserialize.
    /// </typeparam>
    /// <param name="xmlFragment">the raw XML</param>
    public static T? GetInstanceRaw<T>(string? xmlFragment) where T : class
    {
        if (string.IsNullOrWhiteSpace(xmlFragment)) throw new ArgumentNullException(nameof(xmlFragment));

        var serializer = new XmlSerializer(typeof(T));
        using StringReader reader = new StringReader(xmlFragment);
        var instance = serializer.Deserialize(reader) as T;

        return instance;
    }
}
