using System.Text;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="StringBuilder"/>
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends the label with value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static void AppendLabelWithValue(this StringBuilder builder, string name, object value)
        {
            builder.AppendLabelWithValue(name, value, defaultValue: null, hasLineBreak: false);
        }

        /// <summary>
        /// Appends the label with value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        public static void AppendLabelWithValue(this StringBuilder builder, string name, object value, string defaultValue)
        {
            builder.AppendLabelWithValue(name, value, defaultValue, hasLineBreak: false);
        }

        /// <summary>
        /// Appends the label with value.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="hasLineBreak">When <c>true</c> add <see cref="System.Environment.NewLine" /> between label and value.</param>
        /// <remarks>
        /// This method will append <c>name: value</c> to the appending <see cref="StringBuilder"/>.
        /// This is useful when overriding <see cref="object.ToString"/>.
        /// </remarks>
        public static void AppendLabelWithValue(this StringBuilder builder, string name, object value, string defaultValue, bool hasLineBreak)
        {
            if (builder == null) return;
            if ((value == null) && string.IsNullOrWhiteSpace(defaultValue)) return;

            if (hasLineBreak)
            {
                builder.AppendFormat("{0}:", name);
                builder.AppendLine();
                builder.AppendFormat("{0}", value ?? defaultValue);
            }
            else
            {
                builder.AppendFormat("{0}: {1}", name, value ?? defaultValue);
            }

            builder.AppendLine();
        }
    }
}
