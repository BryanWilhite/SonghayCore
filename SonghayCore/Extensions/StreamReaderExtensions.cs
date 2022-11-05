namespace Songhay.Extensions;


/// <summary>
/// Extensions of <see cref="StreamReader"/>.
/// </summary>
/// <remarks>
/// “StreamReader is designed for character input in a particular encoding,
/// whereas the Stream class is designed for byte input and output.”
/// — https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=net-5.0#remarks
/// </remarks>
public static class StreamReaderExtensions
{
    /// <summary>
    /// Reads the lines from the specified <see cref="StreamReader"/>.
    /// </summary>
    /// <param name="reader">the <see cref="StreamReader"/></param>
    /// <param name="lineAction">the line action; return <c>false</c> to stop reading lines</param>
    public static async Task ReadLinesAsync(this StreamReader? reader, Func<string?, bool> lineAction)
    {
        ArgumentNullException.ThrowIfNull(reader);

        while(!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var shouldContinueReadingLines = lineAction?.Invoke(line) ?? false;

            if(!shouldContinueReadingLines) break;
        }
    }
}
