namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="Stream"/>
/// </summary>
/// <remarks>
/// The stream-reader methods in this class work well
/// with <c>Microsoft.AspNetCore.Http.HttpRequest</c>.
/// </remarks>
public static class StreamExtensions
{
    /// <summary>
    /// Reads the specified <see cref="Stream"/> with <see cref="ReadStreamAsStringAsync"/>,
    /// expecting a <see cref="string"/> that can be deserialized as a <see cref="Dictionary{TKey,TValue}"/>
    /// where <c>TKey</c> and <c>TValue</c> are <see cref="string"/> 
    /// or the specified <see cref="ILogger"/> will record an exception.
    /// </summary>
    /// <param name="stream">the <see cref="Stream"/></param>
    /// <param name="logger">The logger.</param>
    public static async Task<Dictionary<string, string>?> ReadStreamIntoDictionaryAsync(this Stream stream, ILogger logger)
    {
        string? json = await stream.ReadStreamAsStringAsync();

        if (string.IsNullOrWhiteSpace(json))
        {
            logger.LogErrorForMissingData("The expected JSON is not here.");

            return null;
        }

        Dictionary<string, string>? args = null;

        try
        {
            args = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }

        return args;
    }

    /// <summary>
    /// Reads the specified <see cref="Stream"/> with <see cref="ReadStreamAsStringAsync"/>,
    /// expecting a <see cref="string"/> that can be parsed into a <see cref="JsonElement"/>
    /// or the specified <see cref="ILogger"/> will record an exception.
    /// </summary>
    /// <param name="stream">the <see cref="Stream"/></param>
    /// <param name="logger">The logger.</param>
    public static async Task<JsonElement> ReadStreamIntoJsonElementAsync(this Stream stream, ILogger logger)
    {
        string? json = await stream.ReadStreamAsStringAsync();

        JsonElement element = JsonElementUtility.GetNullJsonElement();

        if (string.IsNullOrWhiteSpace(json))
        {
            logger.LogErrorForMissingData("The expected JSON is not here.");

            return element;
        }

        try
        {
            element = JsonElement.Parse(json);
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }

        return element;
    }

    /// <summary>
    /// Reads the specified <see cref="Stream"/>,
    /// optimistically calling <see cref="StreamReader.ReadToEndAsync()"/>
    /// </summary>
    /// <param name="stream">the <see cref="Stream"/></param>
    /// <remarks>
    /// This member is likely more memory intensive
    /// than we should see when we look at the Microsoft source code
    /// for <see cref="HttpContent.ReadAsStringAsync()"/>.
    /// </remarks>
    public static async Task<string?> ReadStreamAsStringAsync(this Stream? stream) =>
        stream != null ?
            await new StreamReader(stream).ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false)
            :
            null;
}
