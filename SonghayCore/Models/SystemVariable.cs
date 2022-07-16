namespace Songhay.Models;

/// <summary>
/// Defines the Data for displaying meta-data about variables.
/// </summary>
public class SystemVariable
{
    /// <summary>
    /// Gets or sets the name of the variable.
    /// </summary>
    public string? VariableName { get; set; }

    /// <summary>
    /// Gets or sets the variable description.
    /// </summary>
    public string? VariableDescription { get; set; }

    /// <summary>
    /// Gets or sets the variable value.
    /// </summary>
    public string? VariableValue { get; set; }
}
