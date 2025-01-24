namespace Songhay.Exceptions;

/// <summary>
/// Exception for CSV parsing
/// in <see cref="Songhay.Extensions.StringExtensions.CsvSplit"/>.
/// </summary>
public class CsvParseException : Exception
{
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
    /// <param name="message">The message.</param>
    public CsvParseException(string message) : base(message)
    {
    }
}
