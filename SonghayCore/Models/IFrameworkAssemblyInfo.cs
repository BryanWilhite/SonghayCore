namespace Songhay.Models
{
    /// <summary>
    /// Contract for <see cref="Songhay.Models.FrameworkAssemblyInfo"/>
    /// </summary>
    public interface IFrameworkAssemblyInfo
    {
        /// <summary>
        /// Gets the assembly company.
        /// </summary>
        /// <value>The assembly company.</value>
        string AssemblyCompany { get; }

        /// <summary>
        /// Gets the assembly copyright.
        /// </summary>
        /// <value>The assembly copyright.</value>
        string AssemblyCopyright { get; }

        /// <summary>
        /// Gets the assembly description.
        /// </summary>
        /// <value>The assembly description.</value>
        string AssemblyDescription { get; }

        /// <summary>
        /// Gets the assembly product.
        /// </summary>
        /// <value>The assembly product.</value>
        string AssemblyProduct { get; }

        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <value>The assembly title.</value>
        string AssemblyTitle { get; }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        string AssemblyVersion { get; }

        /// <summary>
        /// Gets the assembly version detail.
        /// </summary>
        /// <value>The assembly version detail.</value>
        string AssemblyVersionDetail { get; }
    }
}
