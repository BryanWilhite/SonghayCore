namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="JsonElement"/>
/// </summary>
public static class JsonElementExtensions
{
    /// <summary>
    /// Displays top-level <see cref="JsonElement"/> properties
    /// without recursion.
    /// </summary>
    /// <param name="element">the <see cref="JsonElement"/></param>
    /// <param name="truncationLength">the number of characters to display for each property</param>
    public static string DisplayTopProperties(this JsonElement element, int truncationLength = 16)
    {
        if (element.ValueKind != JsonValueKind.Object) return $"The expected {nameof(element)} is not here.";

        StringBuilder sb = new();
        foreach (JsonProperty property in element.EnumerateObject())
        {
            var kind = property.Value.ValueKind;

            switch (kind)
            {
                case JsonValueKind.Array:
                case JsonValueKind.Object:
                    sb.AppendLine($"{property.Name}: {property.Value.GetRawText()}");
                    break;

                case JsonValueKind.False:
                case JsonValueKind.Number:
                case JsonValueKind.String:
                case JsonValueKind.True:
                    sb.AppendLine($"{property.Name}: {property.Value.ToStringOrNull()}");
                    break;

                case JsonValueKind.Null:
                    sb.AppendLine($"{property.Name}: {nameof(JsonValueKind.Null).ToLowerInvariant()}");
                    break;

                case JsonValueKind.Undefined:
                    sb.AppendLine($"{property.Name}: {nameof(JsonValueKind.Undefined).ToLowerInvariant()}");
                    break;
            }
        }

        string output = sb.ToString()
            .Replace("{\n", "{")
            .Replace(":\n", ":")
            .Replace(",\n", ",")
            ;

        const string doubleSpaces = "  ";

        while (output.Contains(doubleSpaces))
        {
            output = output.Replace(doubleSpaces, " ");
        }

        return output.Replace("{ \"", "{\"").Truncate(truncationLength) ?? "The expected truncated string is not here.";
    }

    /// <summary>
    /// Tries to return the <see cref="JsonElement" /> property
    /// of the specified <see cref="JsonElement" /> object.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    public static JsonElement? GetJsonChildElementOrNull(this JsonElement? elementOrNull, string? elementName) =>
        elementOrNull?.GetJsonChildElementOrNull(elementName);

    /// <summary>
    /// Tries to return the <see cref="JsonElement" /> property
    /// of the specified <see cref="JsonElement" /> object.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    public static JsonElement? GetJsonChildElementOrNull(this JsonElement element, string? elementName)
    {
        if (element.ValueKind != JsonValueKind.Object) return null;
        if (string.IsNullOrWhiteSpace(elementName)) return null;

        if (!element.TryGetProperty(elementName, out JsonElement property)) return null;

        return property;
    }

    /// <summary>
    /// Returns <c>false</c> when the specified property name does not exist.
    /// </summary>
    /// <param name="elementOrNull">the <see cref="JsonElement"/></param>
    /// <param name="propertyName">the property name</param>
    public static bool HasProperty(this JsonElement? elementOrNull, string? propertyName) =>
        elementOrNull?.HasProperty(propertyName) ?? false;

    /// <summary>
    /// Returns <c>false</c> when the specified property name does not exist.
    /// </summary>
    /// <param name="element">the <see cref="JsonElement"/></param>
    /// <param name="propertyName">the property name</param>
    public static bool HasProperty(this JsonElement element, string? propertyName) => element.GetJsonChildElementOrNull(propertyName) != null;

    /// <summary>
    /// Determines whether the specified <see cref="JsonElement"/>
    /// has the expected properties.
    /// </summary>
    /// <param name="elementOrNull">The node.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="properties">The properties.</param>
    public static bool IsExpectedObject(this JsonElement? elementOrNull, ILogger logger, params string[] properties) =>
        elementOrNull?.IsExpectedObject(logger, properties) ?? false;

    /// <summary>
    /// Determines whether the specified <see cref="JsonElement"/>
    /// has the expected properties.
    /// </summary>
    /// <param name="element">The node.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="properties">The properties.</param>
    public static bool IsExpectedObject(this JsonElement element, ILogger logger, params string[] properties)
    {
        if (element.ValueKind != JsonValueKind.Object) return false;

        bool containsKey = true;

        foreach (var property in properties)
        {
            containsKey = element.HasProperty(property);

            if (containsKey) continue;

            logger.LogError("The expected property, `{Property}`, is not here.", property);

            break;
        }

        return containsKey;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    /// <param name="supportBitStrings">When <c>true</c>, support "1" and "0" as Boolean strings.</param>
    public static bool? ToBooleanOrNull(this JsonElement? elementOrNull, bool supportBitStrings) => elementOrNull?.ToBooleanOrNull(supportBitStrings);

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    /// <param name="supportBitStrings">When <c>true</c>, support "1" and "0" as Boolean strings.</param>
    public static bool? ToBooleanOrNull(this JsonElement element, bool supportBitStrings) =>
        element.ValueKind != JsonValueKind.False
        && element.ValueKind != JsonValueKind.True
        && element.ValueKind != JsonValueKind.String
            ? null
            : ProgramTypeUtility.ParseBoolean(element.ToString(), supportBitStrings);

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="byte"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static byte? ToByteOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToByteOrNull();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="byte"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static byte? ToByteOrNull(this JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Number) return null;
        if (!element.TryGetByte(out byte value)) return null;

        return value;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="DateTime"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static DateTime? ToDateTimeOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToDateTimeOrNull();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static DateTime? ToDateTimeOrNull(this JsonElement element) => ProgramTypeUtility.ParseDateTime(element.ToString());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static decimal? ToDecimalOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToDecimalOrNull();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Decimal"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static decimal? ToDecimalOrNull(this JsonElement element) => ProgramTypeUtility.ParseDecimal(element.ToString());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static double? ToDoubleOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToDoubleOrNull();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Double"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static double? ToDoubleOrNull(this JsonElement element) => ProgramTypeUtility.ParseDouble(element.ToString());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Guid"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static Guid? ToGuidOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToGuidOrNull();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Guid"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static Guid? ToGuidOrNull(this JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.String) return null;
        if (!element.TryGetGuid(out Guid value)) return null;

        return value;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="int"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static int? ToIntOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToIntOrNull();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="int"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static int? ToIntOrNull(this JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Number) return null;
        if (!element.TryGetInt32(out int value)) return null;

        return value;
    }

    /// <summary>
    /// Enumerates the specified <see cref="JsonElement" /> array
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static JsonElement[] ToJsonElementArray(this JsonElement? elementOrNull) => elementOrNull?.ToJsonElementArray() ?? [];

    /// <summary>
    /// Enumerates the specified <see cref="JsonElement" /> array
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static JsonElement[] ToJsonElementArray(this JsonElement elementOrNull)
    {
        JsonElement element = elementOrNull;

        return element.ValueKind != JsonValueKind.Array ? [] : [.. element.EnumerateArray()];
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="long"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static long? ToLongOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToLongOrNull();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="long"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static long? ToLongOrNull(this JsonElement element)
    {
        if (!element.TryGetInt64(out long value)) return null;

        return value;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement"/>
    /// to <c>TObject</c>.
    /// </summary>
    /// <param name="elementOrNull">the specified <see cref="JsonElement"/></param>
    /// <typeparam name="TObject">the reference type to convert to</typeparam>
    /// <remarks>
    /// To clearly express the intent to return value types,
    /// use <see cref="ToValueTypeOrNull{T}(JsonElement?)"/>.
    /// </remarks>
    public static TObject? ToInstanceOrNull<TObject>(this JsonElement? elementOrNull) where TObject : class =>
        elementOrNull?.ToInstanceOrNull<TObject>();

    /// <summary>
    /// Converts the specified <see cref="JsonElement"/>
    /// to <c>TObject</c>.
    /// </summary>
    /// <param name="element">the specified <see cref="JsonElement"/></param>
    /// <typeparam name="TObject">the reference type to convert to</typeparam>
    /// <remarks>
    /// To clearly express the intent to return value types,
    /// use <see cref="ToValueTypeOrNull{T}(JsonElement)"/>.
    /// </remarks>
    public static TObject? ToInstanceOrNull<TObject>(this JsonElement element) where TObject : class => 
        element.ValueKind != JsonValueKind.Object ? null : element.Deserialize<TObject>();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> array
    /// into a collection of string values.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static IReadOnlyCollection<string?> ToReadOnlyCollection(this JsonElement? elementOrNull) => elementOrNull?.ToReadOnlyCollection() ?? [];

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> array
    /// into a collection of string values.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static IReadOnlyCollection<string?> ToReadOnlyCollection(this JsonElement element)
    {
        JsonElement[] elements = element.ToJsonElementArray();

        return elements.Select(el => el.ToStringOrNull()).ToArray();
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> array
    /// into a collection of type <c>T</c>
    /// with the specified converter.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    /// <param name="converter">the specified converter</param>
    public static IReadOnlyCollection<T?> ToReadOnlyCollection<T>(this JsonElement? elementOrNull, Func<JsonElement, T?> converter) => elementOrNull?.ToReadOnlyCollection(converter) ?? [];

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> array
    /// into a collection of type <c>T</c>
    /// with the specified converter.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    /// <param name="converter">the specified converter</param>
    public static IReadOnlyCollection<T?> ToReadOnlyCollection<T>(this JsonElement element, Func<JsonElement, T?> converter)
    {
        JsonElement[] elements = element.ToJsonElementArray();

        return elements.Select(converter).ToArray();
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="short"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static short? ToShortOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToShortOrNull();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="short"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static short? ToShortOrNull(this JsonElement element)
    {
        if (!element.TryGetInt16(out short value)) return null;

        return value;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static string? ToStringOrNull(this JsonElement? elementOrNull) => elementOrNull?.ToStringOrNull(); 

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static string? ToStringOrNull(this JsonElement element) =>
        element.ValueKind switch
        {
            JsonValueKind.True or JsonValueKind.False => element.ToString(),
            JsonValueKind.Number => element.GetRawText(),
            JsonValueKind.String => element.GetString(),
            _ => null
        };

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    /// <remarks>
    /// This member supports structs, including record structs.
    ///
    /// To clearly express the intent to return reference types,
    /// use <see cref="ToInstanceOrNull{T}(JsonElement?)"/>.
    /// </remarks>
    public static T? ToValueTypeOrNull<T>(this JsonElement? elementOrNull) where T : struct => elementOrNull?.ToValueTypeOrNull<T>();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <c>struct</c> value type.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    /// <remarks>
    /// This member supports structs, including record structs.
    ///
    /// To clearly express the intent to return reference types,
    /// use <see cref="ToInstanceOrNull{T}(JsonElement)"/>.
    ///
    /// BTW: I am 98% certain that Microsoft did not design generics to be used with object boxing
    /// which kind of defeats the fundamental purpose of generics.
    /// For higher performance, use methods
    /// like <see cref="ToBooleanOrNull(JsonElement?,bool)"/> instead.
    /// </remarks>
    public static T? ToValueTypeOrNull<T>(this JsonElement element) where T : struct
    {
        return Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Boolean => (T?)(object?)element.ToBooleanOrNull(supportBitStrings: true),
            _ => element.Deserialize<T>()
        };
    }
}
