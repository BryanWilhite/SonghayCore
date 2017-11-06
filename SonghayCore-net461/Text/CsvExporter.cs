using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Songhay.Text
{
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
        public CsvExporter(IEnumerable<T> rows)
            : this(rows, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvExporter{T}"/> class.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <param name="columns">The columns.</param>
        public CsvExporter(IEnumerable<T> rows, IEnumerable<string> columns)
        {
            this.Columns = columns;
            this.Rows = rows;
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        /// <value>
        /// The columns.
        /// </value>
        public IEnumerable<string> Columns { get; private set; }

        /// <summary>
        /// Gets the rows.
        /// </summary>
        /// <value>
        /// The rows.
        /// </value>
        public IEnumerable<T> Rows { get; private set; }

        /// <summary>
        /// Exports this instance.
        /// </summary>
        public string Export()
        {
            return this.Export(true);
        }

        /// <summary>
        /// Exports the specified include header line.
        /// </summary>
        /// <param name="includeHeaderLine">if set to <c>true</c> [include header line].</param>
        public string Export(bool includeHeaderLine)
        {

            StringBuilder sb = new StringBuilder();
            IList<PropertyInfo> propertyInfoList = typeof(T).GetProperties();

            if (includeHeaderLine)
            {
                if (this.Columns != null)
                {
                    foreach (var propertyName in Columns)
                    {
                        sb.Append(propertyName).Append(",");
                    }
                }
                else
                {
                    foreach (PropertyInfo propertyInfo in propertyInfoList)
                    {
                        sb.Append(propertyInfo.Name).Append(",");
                    }
                }
                sb.Remove(sb.Length - 1, 1).AppendLine();
            }

            foreach (T obj in this.Rows)
            {
                if (this.Columns != null)
                {
                    foreach (var propertyName in Columns)
                    {
                        var propertyInfo = propertyInfoList.FirstOrDefault(i => i.Name == propertyName);
                        if (propertyInfo != null) sb.Append(this.MakeCsvText(propertyInfo.GetValue(obj, null))).Append(",");
                    }
                }
                else
                {
                    foreach (var propertyInfo in propertyInfoList)
                    {
                        sb.Append(this.MakeCsvText(propertyInfo.GetValue(obj, null))).Append(",");
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
        public void ExportToFile(string path)
        {
            File.WriteAllText(path, this.Export());
        }

        /// <summary>
        /// Exports to bytes.
        /// </summary>
        public byte[] ExportToBytes()
        {
            return Encoding.UTF8.GetBytes(this.Export());
        }

        string MakeCsvText(object value)
        {
            if (value == null) return string.Empty;

            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd");
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string output = value.ToString();

            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output;

        }
    }
}
