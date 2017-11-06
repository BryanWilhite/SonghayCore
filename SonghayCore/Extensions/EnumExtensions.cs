using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="Enum"/>.
    /// </summary>
    public static class EnumExtensions
    {
#if !NETSTANDARD1_2 && !NETSTANDARD1_4
        /// <summary>
        /// Gets the enum description.
        /// </summary>
        /// <param name="value">The value.</param>
        public static string GetEnumDescription(this Enum value)
        {
            var enumType = value.GetType();
            var enumName = Enum.GetName(enumType, value);
            if (enumName == null) return null;

            FieldInfo field = enumType.GetField(enumName);
            if (field == null) return null;

            var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attr == null) return null;

            return attr.Description;
        }
#endif

        /// <summary>
        /// Gets the enum values.
        /// </summary>
        /// <param name="value">The value.</param>
        public static IEnumerable<Enum> GetEnumValues(this Enum value)
        {
            var enumType = value.GetType();
            var enums = Enum.GetValues(enumType).OfType<Enum>();
            return enums;
        }
    }
}
