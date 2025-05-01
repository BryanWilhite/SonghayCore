namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="JsonElement"/>
/// </summary>
public static class JsonElementExtensions
{
    /// <summary>
    /// Tries to return the <see cref="JsonElement" /> property
    /// of the specified <see cref="JsonElement" /> object.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    public static JsonElement? GetJsonPropertyOrNull(this JsonElement? elementOrNull, string? elementName) =>
        elementOrNull?.GetJsonPropertyOrNull(elementName);

    /// <summary>
    /// Tries to return the <see cref="JsonElement" /> property
    /// of the specified <see cref="JsonElement" /> object.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    public static JsonElement? GetJsonPropertyOrNull(this JsonElement element, string? elementName)
    {
        if (element.ValueKind != JsonValueKind.Object) return null;
        if (string.IsNullOrWhiteSpace(elementName)) return null;

        if (!element.TryGetProperty(elementName, out JsonElement property)) return null;

        return property;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    /// <param name="supportBitStrings">When <c>true</c>, support "1" and "0" as Boolean strings.</param>
    public static bool? ToBoolean(this JsonElement? elementOrNull, bool supportBitStrings) => elementOrNull?.ToBoolean(supportBitStrings);

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    /// <param name="supportBitStrings">When <c>true</c>, support "1" and "0" as Boolean strings.</param>
    public static bool? ToBoolean(this JsonElement element, bool supportBitStrings)
    {
        if (element.ValueKind != JsonValueKind.False
            && element.ValueKind != JsonValueKind.True
            && element.ValueKind != JsonValueKind.String) return null;

        return ProgramTypeUtility.ParseBoolean(element.ToString(), supportBitStrings);
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="byte"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static byte? ToByte(this JsonElement? elementOrNull) => elementOrNull?.ToByte();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="byte"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static byte? ToByte(this JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Number) return null;
        if (!element.TryGetByte(out byte value)) return null;

        return value;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="DateTime"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static DateTime? ToDateTime(this JsonElement? elementOrNull) => elementOrNull?.ToDateTime();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static DateTime? ToDateTime(this JsonElement element) => ProgramTypeUtility.ParseDateTime(element.ToString());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static decimal? ToDecimal(this JsonElement? elementOrNull) => elementOrNull?.ToDecimal();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Decimal"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static decimal? ToDecimal(this JsonElement element) => ProgramTypeUtility.ParseDecimal(element.ToString());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static double? ToDouble(this JsonElement? elementOrNull) => elementOrNull?.ToDouble();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Double"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static double? ToDouble(this JsonElement element) => ProgramTypeUtility.ParseDouble(element.ToString());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Guid"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static Guid? ToGuid(this JsonElement? elementOrNull) => elementOrNull?.ToGuid();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Guid"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static Guid? ToGuid(this JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.String) return null;
        if (!element.TryGetGuid(out Guid value)) return null;

        return value;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="int"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static int? ToInt(this JsonElement? elementOrNull) => elementOrNull?.ToInt();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="int"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static int? ToInt(this JsonElement element)
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
    public static long? ToLong(this JsonElement? elementOrNull) => elementOrNull?.ToLong();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="long"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static long? ToLong(this JsonElement element)
    {
        if (!element.TryGetInt64(out long value)) return null;

        return value;
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement"/>
    /// to <c>TObject</c>.
    /// </summary>
    /// <param name="elementOrNull">the specified <see cref="JsonElement"/></param>
    /// <typeparam name="TObject">the type to convert to</typeparam>
    /// <remarks>
    /// To return value types, use <see cref="ToScalarValue{T}(JsonElement?)"/>.
    /// </remarks>
    public static TObject? ToObject<TObject>(this JsonElement? elementOrNull) where TObject : class =>
        elementOrNull?.ToObject<TObject>();

    /// <summary>
    /// Converts the specified <see cref="JsonElement"/>
    /// to <c>TObject</c>.
    /// </summary>
    /// <param name="element">the specified <see cref="JsonElement"/></param>
    /// <typeparam name="TObject">the type to convert to</typeparam>
    /// <remarks>
    /// To return value types, use <see cref="ToScalarValue{T}(JsonElement)"/>.
    /// </remarks>
    public static TObject? ToObject<TObject>(this JsonElement element) where TObject : class
    {
        if (element.ValueKind != JsonValueKind.Object) return null;

        string json = element.GetRawText();

        return JsonSerializer.Deserialize<TObject>(json);
    }

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

        return elements.Select(el => el.ToStringValue()).ToArray();
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
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static string? ToStringValue(this JsonElement? elementOrNull) => elementOrNull?.ToStringValue(); 

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static string? ToStringValue(this JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.True or
            JsonValueKind.False => element.ToString(),
            JsonValueKind.Number => element.GetRawText(),
            JsonValueKind.String => element.GetString(),
            _ => null
        };
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    /// <remarks>
    /// To de-serialize a class, use <see cref="ToObject{TObject}(JsonElement?)"/>.
    /// </remarks>
    public static T? ToScalarValue<T>(this JsonElement? elementOrNull) where T : struct => elementOrNull?.ToScalarValue<T>();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    /// <remarks>
    /// To de-serialize a class, use <see cref="ToObject{TObject}(JsonElement)"/>.
    ///
    /// BTW: I am 98% certain that Microsoft did not design generics to be used with object boxing
    /// which kind of defeats the fundamental purpose of generics, their use with generic collections.
    /// For higher performance, use methods like <see cref="ToBoolean(System.Text.Json.JsonElement?,bool)"/>
    /// or <see cref="ToDecimal(System.Text.Json.JsonElement)"/> instead.
    /// </remarks>
    public static T? ToScalarValue<T>(this JsonElement element) where T : struct
    {
        object? boxedScalar = element.ValueKind switch
        {
            JsonValueKind.True or
            JsonValueKind.False => element.ToBoolean(supportBitStrings: false),
            JsonValueKind.Number => Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Byte => element.ToByte(),
                TypeCode.Decimal => element.ToDecimal(),
                TypeCode.Double => element.ToDouble(),
                TypeCode.Int32 => element.ToInt(),
                TypeCode.Int64 => element.ToLong(),
                _ => null
            },
            JsonValueKind.String => Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.DateTime => element.GetDateTime(),
                TypeCode.Boolean => element.ToBoolean(supportBitStrings: true),
                _ => null
            },
            _ => null
        };

        return boxedScalar == null ? null : (T)boxedScalar;
    }
}
