namespace Songhay;

/// <summary>
/// Shared methods for <see cref="JsonElement"/>
/// </summary>
public static class JsonElementUtility
{
    /// <summary>
    /// Gets a <see cref="JsonElement"/> where <see cref="JsonValueKind"/>
    /// is <see cref="JsonValueKind.Null"/>.
    /// </summary>
    public static JsonElement GetNullJsonElement() => JsonElement.Parse("null");

    /// <summary>
    /// Parses the specified JSON
    /// or returns a <see cref="JsonElement"/> where <see cref="JsonValueKind"/>
    /// is <see cref="JsonValueKind.Null"/>.
    /// </summary>
    /// <param name="json">the JSON</param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static JsonElement ParseJson(string? json, ILogger logger)
    {
        JsonElement result = GetNullJsonElement();

        if (string.IsNullOrWhiteSpace(json)) return result;

        try
        {
            result = JsonElement.Parse(json);
        }
        catch (Exception ex)
        {
            logger.LogException(ex);
        }

        return result;
    }
}
