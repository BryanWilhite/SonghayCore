namespace Songhay.Extensions;

/// <summary>Extensions of <see cref="Assembly"/>.</summary>
public static class AssemblyExtensions
{
    /// <summary>Gets the path from assembly.</summary>
    /// <param name="assembly">The assembly.</param>
    public static string? GetPathFromAssembly(this Assembly assembly) =>
        ProgramAssemblyUtility.GetPathFromAssembly(assembly);

    /// <summary>Gets the path from assembly.</summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="fileSegment">The file segment.</param>
    public static string GetPathFromAssembly(this Assembly assembly, string fileSegment) =>
        ProgramAssemblyUtility.GetPathFromAssembly(assembly, fileSegment);
}
