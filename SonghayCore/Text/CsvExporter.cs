namespace Songhay.Text;

/// <summary>
/// Transforms and exports the specified class to CSV format.
/// </summary>
/// <typeparam name="T">the class to export</typeparam>
/// <remarks>
/// Based on http://stackoverflow.com/questions/2422212/simple-c-sharp-csv-excel-export-class
/// </remarks>
public class CsvExporter<T> where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CsvExporter{T}"/> class.
    /// </summary>
    /// <param name="rows">The rows.</param>
    public CsvExporter(IEnumerable<T>? rows)
        : this(rows, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CsvExporter{T}"/> class.
    /// </summary>
    /// <param name="rows">The rows.</param>
    /// <param name="columns">The columns.</param>
    public CsvExporter(IEnumerable<T>? rows, IEnumerable<string>? columns)
    {
        Rows = rows ?? Enumerable.Empty<T>();
        Columns = columns ?? Enumerable.Empty<string>();
    }

    /// <summary>
    /// Gets the columns.
    /// </summary>
    public IEnumerable<string> Columns { get; private set; }

    /// <summary>
    /// Gets the rows.
    /// </summary>
    public IEnumerable<T> Rows { get; private set; }

    /// <summary>
    /// Exports this instance.
    /// </summary>
    public string Export()
    {
        return Export(true);
    }

    /// <summary>
    /// Exports the specified include header line.
    /// </summary>
    /// <param name="includeHeaderLine">if set to <c>true</c> [include header line].</param>
    public string Export(bool includeHeaderLine)
    {
        var sb = new StringBuilder();
        IList<PropertyInfo> propertyInfoList = typeof(T).GetProperties();

        if (includeHeaderLine)
        {
            if (Columns.Any())
            {
                foreach (var propertyName in Columns)
                {
                    sb.Append(propertyName).Append(',');
                }
            }
            else
            {
                foreach (var propertyInfo in propertyInfoList)
                {
                    sb.Append(propertyInfo.Name).Append(',');
                }
            }

            sb.Remove(sb.Length - 1, 1).AppendLine();
        }

        foreach (T obj in Rows)
        {
            if (Columns != null)
            {
                foreach (var propertyName in Columns)
                {
                    var propertyInfo = propertyInfoList.FirstOrDefault(i => i.Name == propertyName);
                    if (propertyInfo != null) sb.Append(MakeCsvText(propertyInfo.GetValue(obj, null))).Append(',');
                }
            }
            else
            {
                foreach (var propertyInfo in propertyInfoList)
                {
                    sb.Append(MakeCsvText(propertyInfo.GetValue(obj, null))).Append(',');
                }
            }

            sb.Remove(sb.Length - 1, 1).AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Exports to file.
    /// </summary>
    /// <param name="path">The path.</param>
    public void ExportToFile(string? path) => File.WriteAllText(path.ToReferenceTypeValueOrThrow(), Export());

    /// <summary>
    /// Exports to bytes.
    /// </summary>
    public byte[] ExportToBytes() => Encoding.UTF8.GetBytes(Export());

    static string MakeCsvText(object? value)
    {
        if (value == null) return string.Empty;

        if (value is DateTime time)
        {
            return time.ToString(time.TimeOfDay.TotalSeconds == 0 ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss");
        }

        string output = value.ToString()!;

        if (output.Contains(',') || output.Contains('"'))
            output = $"\"{output.Replace("\"", "\"\"")}\"";

        return output;
    }
}
