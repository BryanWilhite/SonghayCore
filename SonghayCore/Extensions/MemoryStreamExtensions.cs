namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="MemoryStream"/>
/// </summary>
public static class MemoryStreamExtensions
{
    /// <summary>
    /// Converts the specified <see cref="MemoryStream"/>
    /// into a <see cref="Encoding.UTF8"/> string.
    /// </summary>
    /// <param name="stream"><see cref="MemoryStream"/></param>
    public static string? ToUtf8String(this MemoryStream? stream)
    {
        if (stream == null) return null;

        string s = Encoding.UTF8.GetString(stream.ToArray());

        return s.RemoveNullTerminatorCharacters();
    }

    /// <summary>
    /// Converts the specified <see cref="MemoryStream"/>
    /// into a <see cref="Encoding.UTF32"/> string.
    /// </summary>
    /// <param name="stream"><see cref="MemoryStream"/></param>
    public static string? ToUtf32String(this MemoryStream? stream)
    {
        if (stream == null) return null;

        string s = Encoding.UTF32.GetString(stream.ToArray());

        return s.RemoveNullTerminatorCharacters();
    }
}
