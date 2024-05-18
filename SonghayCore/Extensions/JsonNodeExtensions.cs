using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="JsonNode"/>.
/// </summary>
[SuppressMessage("ReSharper", "LogMessageIsSentenceProblem")]
public static class JsonNodeExtensions
{
    /// <summary>
    /// Gets the property value of the specified <see cref="JsonNode"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node">The node.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns></returns>
    public static (T? value, bool success) GetPropertyValue<T>(this JsonNode? node, string propertyName)
    {
        if (node == null) return (default, false);
        if (node.GetJsonValueKind() != JsonValueKind.Object) return (default, false);

        var success = node.AsObject().TryGetPropertyValue(propertyName, out JsonNode? outputNode);

        if (outputNode == null) return (default, success);
        if (outputNode is not JsonValue) return (default, false);

        success = outputNode.AsValue().TryGetValue(out T? value);

        return (value, success);
    }

    /// <summary>
    /// Gets the <see cref="JsonValueKind"/> of the specified <see cref="JsonNode"/>.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <remarks>
    /// This member is needed for .NET 6.0 and earlier.
    /// </remarks>
    public static JsonValueKind GetJsonValueKind(this JsonNode? node)
    {
        return node switch
        {
            null => JsonValueKind.Null,
            JsonArray => JsonValueKind.Array,
            JsonObject => JsonValueKind.Object,
            _ => node is JsonValue ? node.GetValue<JsonElement>().ValueKind : JsonValueKind.Undefined
        };
    }

    /// <summary>
    /// Determines whether the specified <see cref="JsonNode"/>
    /// is the expected <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="properties">The properties.</param>
    public static bool IsExpectedObject(this JsonNode? node, ILogger? logger, params string[] properties)
    {
        if (node == null)
        {
            logger?.LogErrorForMissingData<JsonNode>();

            return false;
        }

        JsonObject? jO = node.ToJsonObject(logger);
        if (jO == null) return false;

        bool containsKey = true;

        foreach (var property in properties)
        {
            containsKey = jO.ContainsKey(property);

            if (containsKey) continue;

            logger?.LogError("The expected property, `{Property}`, is not here.", property);

            break;
        }

        return containsKey;
    }

    /// <summary>
    /// Converts to the specified <see cref="JsonNode"/>
    /// to <see cref="JsonArray"/> or logs failure and returns <c>null</c>.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    public static JsonArray? ToJsonArray(this JsonNode? node, ILogger? logger)
    {
        if (node == null)
        {
            logger?.LogErrorForMissingData<JsonNode>();

            return null;
        }

        if (node.GetJsonValueKind() != JsonValueKind.Array)
        {
            logger?.LogError("The expected {Enum}.{Member} of {Node} is not here.", nameof(JsonValueKind), nameof(JsonValueKind.Object), nameof(JsonNode));

            return null;
        }

        return node.AsArray();
    }

    /// <summary>
    /// Converts to the specified <see cref="JsonNode"/>
    /// to <see cref="JsonObject"/> or logs failure and returns <c>null</c>.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="logger">The logger.</param>
    /// <returns></returns>
    public static JsonObject? ToJsonObject(this JsonNode? node, ILogger? logger)
    {
        if (node == null)
        {
            logger?.LogErrorForMissingData<JsonNode>();

            return null;
        }

        if (node.GetJsonValueKind() != JsonValueKind.Object)
        {
            logger?.LogError("The expected {Enum}.{Member} of {Node} is not here.", nameof(JsonValueKind), nameof(JsonValueKind.Object), nameof(JsonNode));

            return null;
        }

        return node.AsObject();
    }
}
