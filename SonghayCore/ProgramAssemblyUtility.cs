namespace Songhay;

/// <summary>
/// Static members related to <see cref="System.Reflection"/>.
/// </summary>
public static class ProgramAssemblyUtility
{
    /// <summary>
    /// Returns a <see cref="string"/>
    /// about the executing assembly.
    /// </summary>
    /// <param name="targetAssembly">
    /// The executing <see cref="Assembly"/>.
    /// </param>
    /// <returns>Returns <see cref="string"/></returns>
    public static string? GetAssemblyInfo(Assembly targetAssembly) => GetAssemblyInfo(targetAssembly, false);

    /// <summary>
    /// Returns a <see cref="string"/>
    /// about the executing assembly.
    /// </summary>
    /// <param name="targetAssembly">
    /// The executing <see cref="Assembly"/>.
    /// </param>
    /// <param name="useConsoleChars">
    /// When <c>true</c> selected “special” characters are formatted for the Windows Console.
    /// </param>
    /// <returns>Returns <see cref="string"/></returns>
    public static string? GetAssemblyInfo(Assembly? targetAssembly, bool useConsoleChars)
    {
        var sb = new StringBuilder();

        var info = new ProgramAssemblyInfo(targetAssembly);

        sb.Append($"{info.AssemblyTitle} {info.AssemblyVersion}{Environment.NewLine}");
        sb.Append(info.AssemblyDescription);
        sb.Append(Environment.NewLine);
        sb.Append(info.AssemblyCopyright);
        sb.Append(Environment.NewLine);

        return useConsoleChars ? ProgramUtility.GetConsoleCharacters(sb.ToString()) : sb.ToString();
    }

    /// <summary>
    /// Gets the directory name from assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">assembly</exception>
    public static string? GetPathFromAssembly(Assembly? assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var root = Path.GetDirectoryName(assembly.Location);

        return root;
    }

    /// <summary>
    /// Gets the path from assembly.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="fileSegment">The file segment.</param>
    public static string GetPathFromAssembly(Assembly? assembly, string? fileSegment)
    {
        fileSegment.ThrowWhenNullOrWhiteSpace();

        fileSegment = ProgramFileUtility.TrimLeadingDirectorySeparatorChars(fileSegment);

        if (Path.IsPathRooted(fileSegment)) throw new FormatException("The expected relative path is not here.");

        fileSegment = ProgramFileUtility.NormalizePath(fileSegment);

        var root = GetPathFromAssembly(assembly);
        var levels = ProgramFileUtility.CountParentDirectoryChars(fileSegment);
        if (levels > 0) root = ProgramFileUtility.GetParentDirectory(root, levels);

        var path = ProgramFileUtility.GetCombinedPath(root, fileSegment);

        return path;
    }
}
