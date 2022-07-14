using System.ComponentModel;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="Enum"/>.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets any conventional <see cref="DescriptionAttribute.Description"/>
    /// applied to an <see cref="Enum"/>.
    /// </summary>
    /// <param name="value">The <see cref="Enum"/>.</param>
    public static string? GetEnumDescription(this Enum value)
    {
        var enumType = value.GetType();
        var enumName = Enum.GetName(enumType, value);
        if (enumName == null) return null;

        FieldInfo? field = enumType.GetField(enumName);
        if (field == null) return null;

        var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        return attr?.Description;
    }

    /// <summary>
    /// Gets the result of <see cref="Enum.GetValues"/>
    /// based on the specified <see cref="Enum"/>.
    /// </summary>
    /// <param name="value">The <see cref="Enum"/>.</param>
    public static IEnumerable<Enum> GetEnumValues(this Enum value)
    {
        var enumType = value.GetType();
        var enums = Enum.GetValues(enumType).OfType<Enum>();

        return enums;
    }
}
