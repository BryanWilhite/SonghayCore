using System.Text.Json.Nodes;

namespace Songhay;

/// <summary>
/// Shared routines for <see cref="JsonNode"/>
/// </summary>
public static class JsonNodeUtility
{
    /// <summary>
    /// Converts the specified <see cref="object"/>
    /// to a <see cref="JsonNode"/>.
    /// </summary>
    /// <param name="data">any <see cref="object"/></param>
    /// <remarks>
    /// The primary inspiration for this member is
    /// to transform an anonymous object to a <see cref="JsonNode"/>.
    /// </remarks>
    public static JsonNode? ConvertToJsonNode(object? data)
    {
        if (data == null) return JsonNode.Parse("{}");

        string json = JsonSerializer.Serialize(data);

        return JsonNode.Parse(json);
    }

    /// <summary>
    /// A wrapper for <see cref="JsonNode.Parse(System.IO.Stream,System.Text.Json.Nodes.JsonNodeOptions?,System.Text.Json.JsonDocumentOptions)"/>
    /// </summary>
    /// <param name="json">the JSON to parse</param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static JsonObject? ParseJsonObject(string json, ILogger logger)
    {
        JsonObject? jObject = null;
        try
        {
            jObject = JsonNode.Parse(json)?.AsObject();
        }
        catch (JsonException ex)
        {
            logger.LogError("Tried to parse JSON.\n{Message}", ex.Message);
        }

        return jObject;
    }
}
