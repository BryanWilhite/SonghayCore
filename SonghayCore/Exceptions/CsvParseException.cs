using System;
using System.Runtime.Serialization;

namespace Songhay
{
    /// <summary>
    /// Exception for CSV parsing
    /// in <see cref="Songhay.Extensions.StringExtensions.CsvSplit"/>.
    /// </summary>
    [Serializable]
    public class CsvParseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        public CsvParseException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CsvParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected CsvParseException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvParseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CsvParseException(string message) : base(message) { }
    }
}
