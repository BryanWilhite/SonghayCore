using System.Data.Common;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="DbDataReader"/>
/// </summary>
public static class DbDataReaderExtensions
{
    /// <summary>
    /// Gets the field value or default.
    /// </summary>
    /// <typeparam name="TRow"></typeparam>
    /// <param name="reader">The reader.</param>
    /// <param name="fieldName">Name of the field.</param>
    /// <remarks>
    /// This member is a primitive alternative to using something like Dapper’s
    /// <c>QueryAsync{TRow}</c>, suitable for a handful of columns.
    /// </remarks>
    public static TRow? GetFieldValueOrDefault<TRow>(this DbDataReader reader, string fieldName)
    {
        int index = reader.GetOrdinal(fieldName);

        return reader.IsDBNull(index) ? default : reader.GetFieldValue<TRow>(index);
    }
}
