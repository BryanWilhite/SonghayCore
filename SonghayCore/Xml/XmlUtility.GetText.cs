namespace Songhay.Xml;

public static partial class XmlUtility
{
    /// <summary>
    /// “Cleans” XML data returning
    /// in a <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="ramStream"><see cref="MemoryStream"/></param>
    public static string? GetText(MemoryStream? ramStream)
    {
        if (ramStream == null) return null;

        var s = Encoding.UTF8.GetString(ramStream.ToArray());

        if (!string.IsNullOrWhiteSpace(s))
            s = s.Trim().Replace("\0", string.Empty);

        return s;
    }
}
