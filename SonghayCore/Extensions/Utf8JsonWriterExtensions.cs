using System;
using System.Text.Json;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="Utf8JsonWriter"/>
    /// </summary>
    public static class Utf8JsonWriterExtensions
    {
        /// <summary>
        /// Wrap <see cref="Utf8JsonWriter"/> statements
        /// inside <see cref="Utf8JsonWriter.WriteStartObject"/>
        /// and <see cref="Utf8JsonWriter.WriteEndObject"/>.
        /// </summary>
        /// <param name="writer">the <see cref="Utf8JsonWriter"/></param>
        /// <param name="writerAction"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method is for building a JSON object.
        /// </remarks>
        public static Utf8JsonWriter WriteObject(this Utf8JsonWriter writer, Action writerAction)
        {
            if (writer == null) return writer;

            writer.WriteStartObject();
            writerAction?.Invoke();
            writer.WriteEndObject();

            return writer;
        }

        /// <summary>
        /// Wrap <see cref="Utf8JsonWriter"/> statements
        /// inside <see cref="Utf8JsonWriter.WritePropertyName"/>
        /// <see cref="Utf8JsonWriter.WriteStartObject"/>
        /// and <see cref="Utf8JsonWriter.WriteEndObject"/>.
        /// </summary>
        /// <param name="writer">the <see cref="Utf8JsonWriter"/></param>
        /// <param name="propertyName"></param>
        /// <param name="writerAction"></param>
        /// <returns></returns>
        /// <remarks>
        /// This method is for building a JSON object for a JSON property.
        /// </remarks>
        public static Utf8JsonWriter WriteObject(this Utf8JsonWriter writer, string propertyName, Action writerAction)
        {
            if (writer == null) return writer;
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            writer.WritePropertyName(propertyName);
            writer.WriteStartObject();
            writerAction?.Invoke();
            writer.WriteEndObject();

            return writer;
        }
    }
}