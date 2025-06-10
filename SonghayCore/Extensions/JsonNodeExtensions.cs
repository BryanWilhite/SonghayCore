using System.Text.Json.Nodes;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="JsonNode"/>.
/// </summary>
/// <remarks>
/// To prevent passing null instances of <see cref="ILogger"/> into these methods,
/// use <see cref="ILoggerUtility.AsInstanceOrNullLogger"/>.
/// </remarks>
[SuppressMessage("ReSharper", "LogMessageIsSentenceProblem")]
public static class JsonNodeExtensions
{
    /// <summary>
    /// Displays top-level <see cref="JsonObject"/> properties
    /// without recursion.
    /// </summary>
    /// <param name="jObject">the <see cref="JsonObject"/></param>
    /// <param name="truncationLength">the number of characters to display for each property</param>
    public static string DisplayTopProperties(this JsonObject? jObject, int truncationLength = 16)
    {
        if (jObject == null) return $"The expected {nameof(jObject)} is not here.";

        StringBuilder sb = new();
        foreach (KeyValuePair<string, JsonNode?> pair in jObject)
        {
            JsonValueKind kind = pair.Value?.GetValueKind() ?? JsonValueKind.Null;

            switch (kind)
            {
                case JsonValueKind.Array:
                case JsonValueKind.Object:
                    sb.AppendLine($"{pair.Key}: {pair.Value?.ToJsonString().Truncate(truncationLength)}");
                    break;

                case JsonValueKind.False:
                case JsonValueKind.Number:
                case JsonValueKind.String:
                case JsonValueKind.True:
                    sb.AppendLine($"{pair.Key}: {pair.Value?.AsValue().ToString()}");
                    break;

                case JsonValueKind.Null:
                    sb.AppendLine($"{pair.Key}: {nameof(JsonValueKind.Null).ToLowerInvariant()}");
                    break;

                case JsonValueKind.Undefined:
                    sb.AppendLine($"{pair.Key}: {nameof(JsonValueKind.Undefined).ToLowerInvariant()}");
                    break;
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets the <see cref="JsonValue"/> of the specified <see cref="JsonNode"/>
    /// of <see cref="JsonValueKind.Array" />
    /// or defaults to null.
    /// </summary>
    /// <param name="node">The <see cref="JsonNode"/>.</param>
    /// <param name="propertyName">Name of the property.</param>
    public static JsonArray? GetPropertyJsonArrayOrNull(this JsonNode? node, string propertyName)
    {
        if (node == null) return null;
        if (node.GetValueKind() != JsonValueKind.Object) return null;

        if (!node.AsObject().TryGetPropertyValue(propertyName, out JsonNode? outputNode)) return null;

        return outputNode is not JsonArray ? null : outputNode.AsArray();
    }

    /// <summary>
    /// Gets the <see cref="JsonValue"/> of the specified <see cref="JsonNode"/>
    /// of <see cref="JsonValueKind.Object" />
    /// or defaults to null.
    /// </summary>
    /// <param name="node">The <see cref="JsonNode"/>.</param>
    /// <param name="propertyName">Name of the property.</param>
    public static JsonObject? GetPropertyJsonObjectOrNull(this JsonNode? node, string propertyName)
    {
        if (node == null) return null;
        if (node.GetValueKind() != JsonValueKind.Object) return null;

        if (!node.AsObject().TryGetPropertyValue(propertyName, out JsonNode? outputNode)) return null;

        return outputNode is not JsonObject ? null : outputNode.AsObject();
    }

    /// <summary>
    /// Returns the <see cref="JsonNode"/>
    /// of the specified target property name
    /// or <c>null</c>.
    /// </summary>
    /// <param name="jObject">the <see cref="JsonObject"/></param>
    /// <param name="targetPropertyName">the target property name</param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static JsonNode? GetPropertyJsonNodeOrNull(this JsonObject? jObject, string targetPropertyName, ILogger logger)
    {
        logger.LogTraceMethodCall(nameof(GetPropertyJsonNodeOrNull));

        if (jObject == null)
        {
            logger.LogDebug("The expected parent object of node, `{Name}`, is not here.", targetPropertyName);

            return null;
        }

        if (!jObject.TryGetPropertyValue(targetPropertyName, out JsonNode? targetNode) || targetNode == null)
        {
            logger.LogDebug("The expected node, `{Name}`, is not here.", targetPropertyName);

            return null;
        }

        return targetNode;
    }

    /// <summary>
    /// Gets the <see cref="JsonValue"/> of the specified <see cref="JsonNode"/>
    /// or defaults to null.
    /// </summary>
    /// <param name="node">The <see cref="JsonNode"/>.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <remarks>
    /// This member will return <c>null</c> when the specified <see cref="JsonNode"/>
    /// is not of:
    /// * <see cref="JsonValueKind.String" />
    /// * <see cref="JsonValueKind.Number" />
    /// * <see cref="JsonValueKind.True" />
    /// * <see cref="JsonValueKind.False" />
    /// </remarks>
    public static JsonValue? GetPropertyJsonValueOrNull(this JsonNode? node, string propertyName)
    {
        if (node == null) return null;
        if (node.GetValueKind() != JsonValueKind.Object) return null;

        if (!node.AsObject().TryGetPropertyValue(propertyName, out JsonNode? outputNode)) return null;

        return outputNode is not JsonValue ? null : outputNode.AsValue();
    }

    /// <summary>
    /// Gets the property value of the specified <see cref="JsonNode"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node">The node.</param>
    /// <param name="propertyName">Name of the property.</param>
    [Obsolete(message: "Use `GetPropertyJsonArrayOrNull`, `GetPropertyJsonObjectOrNull` or `GetPropertyJsonValueOrNull` instead.")]
    public static (T? value, bool success) GetPropertyValue<T>(this JsonNode? node, string propertyName)
    {
        if (node == null) return (default, false);
        if (node.GetValueKind() != JsonValueKind.Object) return (default, false);

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
    [Obsolete("For .NET 8 and beyond use `JsonNode.GetValueKind()` instead.")]
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
    /// Returns <c>false</c> when the specified property name does not exist.
    /// </summary>
    /// <param name="node">the <see cref="JsonNode"/></param>
    /// <param name="propertyName">the property name</param>
    public static bool HasProperty(this JsonNode? node, string? propertyName)
    {
        if (node == null) return false;
        if (string.IsNullOrWhiteSpace(propertyName)) return false;

        return node[propertyName] != null;
    }

    /// <summary>
    /// Determines whether the specified <see cref="JsonNode"/>
    /// is the expected <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="properties">The properties.</param>
    public static bool IsExpectedObject(this JsonNode? node, ILogger logger, params string[] properties)
    {
        if (node == null)
        {
            logger.LogErrorForMissingData<JsonNode>();

            return false;
        }

        JsonObject? jO = node.ToJsonObject(logger);
        if (jO == null) return false;

        bool containsKey = true;

        foreach (var property in properties)
        {
            containsKey = jO.ContainsKey(property);

            if (containsKey) continue;

            logger.LogError("The expected property, `{Property}`, is not here.", property);

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
    public static JsonArray? ToJsonArray(this JsonNode? node, ILogger logger)
    {
        if (node == null)
        {
            logger.LogErrorForMissingData<JsonNode>();

            return null;
        }

        if (node.GetValueKind() != JsonValueKind.Array)
        {
            logger.LogError("The kind of JSON, {Enum}.{Member}, of the specified {Node} is not expected.", nameof(JsonValueKind), nameof(JsonValueKind.Object), nameof(JsonNode));

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
    public static JsonObject? ToJsonObject(this JsonNode? node, ILogger logger)
    {
        if (node == null)
        {
            logger.LogErrorForMissingData<JsonNode>();

            return null;
        }

        if (node.GetValueKind() != JsonValueKind.Object)
        {
            logger.LogError("The kind of JSON, {Enum}.{Member}, of the specified {Node} is not expected.", nameof(JsonValueKind), nameof(JsonValueKind.Object), nameof(JsonNode));

            return null;
        }

        return node.AsObject();
    }

    /// <summary>
    /// Removes the specified <see cref="JsonNode"/>
    /// when its parent is <see cref="JsonArray"/> or <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="node">the <see cref="JsonNode"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static void RemoveFromParent(this JsonNode? node, ILogger logger) => RemoveFromParent(node, propertyName: null, logger);

    /// <summary>
    /// Removes the specified <see cref="JsonNode"/>
    /// when its parent is <see cref="JsonArray"/> or <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="node">the <see cref="JsonNode"/></param>
    /// <param name="propertyName">the name of the property when the specified node is <see cref="JsonValueKind.Object"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static void RemoveFromParent(this JsonNode? node, string? propertyName, ILogger logger)
    {
        if (node == null)
        {
            logger.LogWarning("{Name}: The expected node is not here.", nameof(RemoveFromParent));

            return;
        }

        if (node.Parent == null)
        {
            logger.LogWarning("{Name}: This node has no parent.", nameof(RemoveFromParent));

            return;
        }

        var kind = node.Parent.GetValueKind();

        switch (kind)
        {
            case JsonValueKind.Array:
                node.Parent.AsArray().Remove(node);
                break;

            case JsonValueKind.Object:
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    logger.LogDebug("{Kind} requires a property name.", kind.ToString());

                    break;
                }

                node.Parent.AsObject().Remove(propertyName);
                break;

            default:
                logger.LogDebug("{Kind} is not supported.", kind.ToString());
                break;
        }
    }

    /// <summary>
    /// Removes the specified <see cref="JsonNode"/>
    /// when its parent is <see cref="JsonArray"/> or <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="node">the <see cref="JsonNode"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static void RemoveProperty(this JsonNode? node, ILogger logger) => RemoveProperty(node, propertyName: null, logger);

    /// <summary>
    /// Removes the specified <see cref="JsonNode"/>
    /// with the specified property name
    /// when the node is <see cref="JsonObject"/>
    /// or <see cref="JsonArray"/> (array of <see cref="JsonObject"/> with a property to remove).
    /// </summary>
    /// <param name="node">the <see cref="JsonNode"/></param>
    /// <param name="propertyName">the name of the property when the specified node is <see cref="JsonValueKind.Object"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    public static void RemoveProperty(this JsonNode? node, string? propertyName, ILogger logger)
    {
        if (node == null)
        {
            logger.LogDebug("{Name}: The expected node is not here.", nameof(RemoveProperty));

            return;
        }

        JsonValueKind kind = node.GetValueKind();

        switch (kind)
        {
            case JsonValueKind.Array:
                node.AsArray().ForEachInEnumerable(n => n.RemoveProperty(propertyName, logger));
                break;

            case JsonValueKind.Object:
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    break;
                }

                node.AsObject().Remove(propertyName);
                break;

            default:
                logger.LogWarning("{Kind} is not supported.", kind.ToString());
                break;
        }
    }

    /// <summary>
    /// Returns the specified <see cref="JsonObject"/>
    /// with its properties renamed.
    /// </summary>
    /// <param name="documentData">the <see cref="JsonObject"/></param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    /// <param name="operations">specifies which <see cref="JsonObject"/> properties to rename</param>
    public static JsonObject? WithPropertiesRenamed(this JsonObject? documentData, ILogger logger, params (string oldName, string newName)[] operations)
    {
        if (documentData == null) return documentData;
        foreach (var (oldName, newName) in operations)
        {
            if(!documentData.HasProperty(oldName)) continue;

            logger.LogDebug("Renaming `{OldName}` property to `{NewName}`...", oldName, newName);

            JsonNode? oldNode = documentData[oldName];
            if (oldNode == null)
            {
                logger.LogWarning("Warning: the expected element, `{OldName}`, is not here. Continuing...", oldName);

                continue;
            }

            documentData[newName] = oldNode.DeepClone();
            documentData.Remove(oldName);
        }

        return documentData;
    }
}
