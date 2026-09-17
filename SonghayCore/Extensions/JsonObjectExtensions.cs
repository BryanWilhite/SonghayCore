using System.Text.Json.Nodes;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="JsonNode"/>.
/// </summary>
/// <remarks>
/// To prevent passing null instances of <see cref="ILogger"/> into these methods,
/// use <see cref="ILoggerUtility.AsInstanceOrNullLogger"/>.
/// </remarks>
public static class JsonObjectExtensions
{
    /// <summary>
    /// Adds the specified <c>item</c>
    /// to the <see cref="JsonArray"/>
    /// of the specified <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="jsonObject">the <see cref="JsonObject"/></param>
    /// <param name="arrayPropertyName">the name of the <see cref="JsonArray"/> property</param>
    /// <param name="item">the item to add</param>
    /// <param name="logger">the <see cref="ILogger"/></param>
    /// <typeparam name="TItem">the type of the item to add</typeparam>
    public static void AddItemToArray<TItem>(this JsonObject? jsonObject, string? arrayPropertyName, TItem? item, ILogger logger)
    {
        if (jsonObject == null) return;
        if (string.IsNullOrWhiteSpace(arrayPropertyName)) return;

        if (jsonObject[arrayPropertyName] is JsonArray itemsArray)
        {
            itemsArray.Add(item);
        }
        else
        {
            logger.LogWarning("Unable to add item to property `{Name}`. Is this an array?", arrayPropertyName);
        }
    }

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
