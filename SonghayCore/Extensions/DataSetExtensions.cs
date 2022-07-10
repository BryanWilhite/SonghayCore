using System.Data;

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="System.Data.DataSet"/>
/// </summary>
public static class DataSetExtensions
{
    /// <summary>
    /// Converts the <see cref="DataSet"/> into a first table data rows.
    /// </summary>
    /// <param name="dataSet">The data set.</param>
    public static IEnumerable<DataRow> ToFirstTableDataRows(this DataSet? dataSet)
    {
        if (dataSet == null) return Enumerable.Empty<DataRow>();

        var table = dataSet
            .Tables.OfType<DataTable>()
            .FirstOrDefault();

        return table == null ? Enumerable.Empty<DataRow>() : table.Rows.OfType<DataRow>();
    }

    /// <summary>
    /// Converts the <see cref="DataSet"/> into a first table first column.
    /// </summary>
    /// <param name="dataSet">The data set.</param>
    public static IEnumerable<string> ToFirstTableFirstColumn(this DataSet? dataSet) =>
        dataSet.ToFirstTableFirstColumn<string>();

    /// <summary>
    /// Converts the <see cref="DataSet"/> into a first table first column.
    /// </summary>
    /// <typeparam name="TColumn">The type of the column.</typeparam>
    /// <param name="dataSet">The data set.</param>
    public static IEnumerable<TColumn> ToFirstTableFirstColumn<TColumn>(this DataSet? dataSet)
    {
        var rows = dataSet.ToFirstTableDataRows();

        return rows.Select(i => (TColumn) i[0]);
    }

    /// <summary>
    /// Converts the <see cref="DataSet"/> into a first table rows.
    /// </summary>
    /// <param name="dataSet">The data set.</param>
    public static IEnumerable<KeyValuePair<string, string>> ToFirstTableRows(this DataSet? dataSet) =>
        dataSet.ToFirstTableRows<string, string>();

    /// <summary>
    /// Converts the <see cref="DataSet"/> into a first table rows.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="dataSet">The data set.</param>
    public static IEnumerable<KeyValuePair<TKey, TValue>> ToFirstTableRows<TKey, TValue>(this DataSet? dataSet)
    {
        var rows = dataSet.ToFirstTableDataRows();

        return rows.Select(i => new KeyValuePair<TKey, TValue>((TKey) i[0], (TValue) i[1]));
    }
}
