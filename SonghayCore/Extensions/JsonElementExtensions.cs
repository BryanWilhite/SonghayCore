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

        return ProgramTypeUtility.ParseBoolean(element.GetRawText(), supportBitStrings);
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
    public static DateTime? ToDateTime(this JsonElement element) => ProgramTypeUtility.ParseDateTime(element.GetRawText());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Decimal"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static decimal? ToDecimal(this JsonElement element) => ProgramTypeUtility.ParseDecimal(element.GetRawText());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static decimal? ToDecimal(this JsonElement? elementOrNull) => elementOrNull?.ToDecimal();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="Double"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static double? ToDouble(this JsonElement element) => ProgramTypeUtility.ParseDouble(element.GetRawText());

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="bool"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static double? ToDouble(this JsonElement? elementOrNull) => elementOrNull?.ToDouble();

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
            JsonValueKind.True => element.GetRawText(),
            JsonValueKind.False => element.GetRawText(),
            JsonValueKind.Number => element.GetRawText(),
            JsonValueKind.String => element.GetString(),
            _ => null
        };
    }

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="elementOrNull">The <see cref="JsonElement" />.</param>
    public static T? ToScalarValue<T>(this JsonElement? elementOrNull) where T : struct => elementOrNull?.ToScalarValue<T>();

    /// <summary>
    /// Converts the specified <see cref="JsonElement" /> into a nullable <see cref="string"/>.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    public static T? ToScalarValue<T>(this JsonElement element) where T : struct
    {
        object? boxedScalar = element.ValueKind switch
        {
            JsonValueKind.True => element.ToBoolean(supportBitStrings: false),
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
