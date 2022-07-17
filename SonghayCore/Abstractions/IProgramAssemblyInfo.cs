namespace Songhay.Abstractions;

/// <summary>
/// Contract for <see cref="Songhay.Models.ProgramAssemblyInfo"/>
/// </summary>
public interface IProgramAssemblyInfo
{
    /// <summary>
    /// Gets the assembly company.
    /// </summary>
    string? AssemblyCompany { get; }

    /// <summary>
    /// Gets the assembly copyright.
    /// </summary>
    string? AssemblyCopyright { get; }

    /// <summary>
    /// Gets the assembly description.
    /// </summary>
    string? AssemblyDescription { get; }

    /// <summary>
    /// Gets the assembly product.
    /// </summary>
    string? AssemblyProduct { get; }

    /// <summary>
    /// Gets the assembly title.
    /// </summary>
    string? AssemblyTitle { get; }

    /// <summary>
    /// Gets the assembly version.
    /// </summary>
    string? AssemblyVersion { get; }

    /// <summary>
    /// Gets the assembly version detail.
    /// </summary>
    string? AssemblyVersionDetail { get; }
}
