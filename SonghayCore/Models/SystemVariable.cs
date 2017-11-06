
namespace Songhay.Models
{
    /// <summary>
    /// Defines the Data for displaying meta-data about variables.
    /// </summary>
    public class SystemVariable
    {
        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        /// <value>The name of the variable.</value>
        public string VariableName { get; set; }

        /// <summary>
        /// Gets or sets the variable description.
        /// </summary>
        /// <value>The variable description.</value>
        public string VariableDescription { get; set; }

        /// <summary>
        /// Gets or sets the variable value.
        /// </summary>
        /// <value>The variable value.</value>
        public string VariableValue { get; set; }
    }
}
