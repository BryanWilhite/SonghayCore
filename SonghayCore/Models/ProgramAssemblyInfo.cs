namespace Songhay.Models;

/// <summary>
/// Defines Assembly information.
/// </summary>
/// <remarks>
/// This definition was developed
/// for About… dialogs in Windows Forms.
/// </remarks>
public class ProgramAssemblyInfo : IProgramAssemblyInfo
{
    /// <summary>
    /// Constructor of this class.
    /// </summary>
    /// <param name="targetAssembly">The target <see cref="System.Reflection.Assembly"/></param>
    public ProgramAssemblyInfo(Assembly? targetAssembly)
    {
        ArgumentNullException.ThrowIfNull(targetAssembly);

        _dll = targetAssembly;
    }

    /// <summary>
    /// Gets title of assembly.
    /// </summary>
    public string AssemblyTitle
    {
        get
        {
            object[] attributes = _dll.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            if (attributes.Length <= 0) return Path.GetFileNameWithoutExtension(_dll.Location);

            var titleAttribute = attributes[0] as AssemblyTitleAttribute;

            return string.IsNullOrWhiteSpace(titleAttribute?.Title)
                ? Path.GetFileNameWithoutExtension(_dll.Location)
                : titleAttribute.Title;
        }
    }

    /// <summary>
    /// Gets Assembly version information.
    /// </summary>
    public string AssemblyVersion
    {
        get
        {
            AssemblyName name = _dll.GetName();

            return name.Version?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets detailed Assembly version information.
    /// </summary>
    public string AssemblyVersionDetail
    {
        get
        {
            AssemblyName dllName = _dll.GetName();

            return $"{dllName.Version?.Major ?? 0:D}.{dllName.Version?.Minor ?? 0:D2}";
        }
    }

    /// <summary>
    /// Gets Assembly description information.
    /// </summary>
    public string AssemblyDescription
    {
        get
        {
            object[] attributes = _dll.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

            return attributes.Length == 0
                ? string.Empty
                : (attributes[0] as AssemblyDescriptionAttribute)?.Description ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets Assembly product information.
    /// </summary>
    public string AssemblyProduct
    {
        get
        {
            object[] attributes = _dll.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

            return attributes.Length == 0 ? string.Empty : ((AssemblyProductAttribute) attributes[0]).Product;
        }
    }

    /// <summary>
    /// Gets Assembly copyright information.
    /// </summary>
    public string AssemblyCopyright
    {
        get
        {
            object[] attributes = _dll.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
        }
    }

    /// <summary>
    /// Gets Assembly company information.
    /// </summary>
    public string AssemblyCompany
    {
        get
        {
            object[] attributes = _dll.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

            return attributes.Length == 0 ? string.Empty : ((AssemblyCompanyAttribute) attributes[0]).Company;
        }
    }

    /// <summary>
    /// Returns format: <c>[AssemblyCompany], [AssemblyTitle] Version: [AssemblyVersion], [AssemblyVersionDetail]</c>.
    /// </summary>
    public override string ToString()
    {
        string s = $"{AssemblyCompany}, {AssemblyTitle} Version: {AssemblyVersion}, {AssemblyVersionDetail}";

        return s;
    }

    readonly Assembly _dll;
}
