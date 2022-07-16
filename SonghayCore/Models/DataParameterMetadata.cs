using System.Data;

namespace Songhay.Models;

/// <summary>
/// A JSON.net-friendly definition for types implementing <see cref="IDataParameter"/>.
/// </summary>
public class DataParameterMetadata
{
    /// <summary>
    /// Gets or sets the data row version.
    /// </summary>
    public DataRowVersion DataRowVersion { get; set; } = DataRowVersion.Default;

    /// <summary>
    /// Gets or sets the type of the database.
    /// </summary>
    public DbType DbType { get; set; }

    /// <summary>
    /// Gets or sets the parameter direction.
    /// </summary>
    public ParameterDirection ParameterDirection { get; set; } = ParameterDirection.Input;

    /// <summary>
    /// Gets or sets the name of the parameter.
    /// </summary>
    public string? ParameterName { get; set; }

    /// <summary>
    /// Gets or sets the parameter value.
    /// </summary>
    public object? ParameterValue { get; set; }

    /// <summary>
    /// Gets or sets the source column.
    /// </summary>
    public string? SourceColumn { get; set; }
}
