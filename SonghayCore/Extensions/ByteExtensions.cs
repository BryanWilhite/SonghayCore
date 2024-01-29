namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="byte"/>.
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// Converts the specified <see cref="byte"/> array to a <see cref="StreamReader"/>
    /// with <see cref="Encoding.UTF8"/>.
    /// </summary>
    /// <param name="bytes">the bytes</param>
    public static StreamReader ToStreamReader(this byte[] bytes) => bytes.ToStreamReader(Encoding.UTF8);

    /// <summary>
    /// Converts the specified <see cref="byte"/> array to a <see cref="StreamReader"/>.
    /// </summary>
    /// <param name="bytes">the bytes</param>
    /// <param name="encoding">the encoding</param>
    /// <remarks>
    /// This member is useful in the world of ASP.NET when there is a need
    /// to read a <see cref="string"/> from the bytes of a <c>MultipartSection"</c>.
    /// </remarks>
    public static StreamReader ToStreamReader(this byte[] bytes, Encoding encoding) =>
        new(new MemoryStream(bytes), encoding);
}
