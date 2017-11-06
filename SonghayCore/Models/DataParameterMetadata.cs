using System.Data;

namespace Songhay.Models
{
    /// <summary>
    /// A JSON.net-friendly definition for types implementing <see cref="IDataParameter"/>.
    /// </summary>
    public class DataParameterMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataParameterMetadata"/> class.
        /// </summary>
        public DataParameterMetadata()
        {
            this.DataRowVersion = DataRowVersion.Default;
            this.ParameterDirection = ParameterDirection.Input;
        }

        /// <summary>
        /// Gets or sets the data row version.
        /// </summary>
        /// <value>
        /// The data row version.
        /// </value>
        public DataRowVersion DataRowVersion { get; set; }

        /// <summary>
        /// Gets or sets the type of the database.
        /// </summary>
        /// <value>
        /// The type of the database.
        /// </value>
        public DbType DbType { get; set; }

        /// <summary>
        /// Gets or sets the parameter direction.
        /// </summary>
        /// <value>
        /// The parameter direction.
        /// </value>
        public ParameterDirection ParameterDirection { get; set; }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName { get; set; }
        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>
        /// The parameter value.
        /// </value>
        public object ParameterValue { get; set; }

        /// <summary>
        /// Gets or sets the source column.
        /// </summary>
        /// <value>
        /// The source column.
        /// </value>
        public string SourceColumn { get; set; }
    }
}
