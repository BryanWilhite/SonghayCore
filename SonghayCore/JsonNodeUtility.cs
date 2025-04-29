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
}
