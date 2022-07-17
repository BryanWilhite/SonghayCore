namespace Songhay.Extensions;

/// <summary>Extensions of <see cref="DirectoryInfo"/>.</summary>
public static class DirectoryInfoExtensions
{
    /// <summary>
    /// Finds the specified target <see cref="DirectoryInfo"/>
    /// under the specified root <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <param name="directoryInfo">The specified root <see cref="DirectoryInfo"/>.</param>
    /// <param name="expectedDirectoryName">The specified target <see cref="DirectoryInfo.Name"/>.</param>
    public static DirectoryInfo? FindDirectory(this DirectoryInfo? directoryInfo, string? expectedDirectoryName)
    {
        expectedDirectoryName.ThrowWhenNullOrWhiteSpace();

        if (directoryInfo == null)
            throw new DirectoryNotFoundException("The expected root directory is not here.");

        if (!directoryInfo.Exists)
            throw new DirectoryNotFoundException("The expected root directory does not exist.");

        return directoryInfo.GetDirectories(expectedDirectoryName).FirstOrDefault();
    }

    /// <summary>
    /// Finds the specified <see cref="FileInfo"/>
    /// under the specified <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <param name="directoryInfo">The specified <see cref="DirectoryInfo"/>.</param>
    /// <param name="expectedFileName">The specified <see cref="FileInfo.Name"/>.</param>
    public static FileInfo? FindFile(this DirectoryInfo? directoryInfo, string? expectedFileName)
    {
        expectedFileName.ThrowWhenNullOrWhiteSpace();

        if (directoryInfo == null)
            throw new DirectoryNotFoundException("The expected directory is not here.");

        if (!directoryInfo.Exists)
            throw new DirectoryNotFoundException("The expected directory does not exist.");

        return directoryInfo.GetFiles(expectedFileName).FirstOrDefault();
    }

    /// <summary>Gets the parent directory.</summary>
    /// <param name="directoryInfo">The specified <see cref="DirectoryInfo"/>.</param>
    /// <param name="levels">The levels.</param>
    /// <returns>Returns a <see cref="string"/> representing the directory.</returns>
    public static string? GetParentDirectory(this DirectoryInfo? directoryInfo, int levels) =>
        directoryInfo.GetParentDirectoryInfo(levels)?.FullName;

    /// <summary>Gets the parent <see cref="DirectoryInfo"/>.</summary>
    /// <param name="directoryInfo">The specified <see cref="DirectoryInfo"/>.</param>
    /// <param name="levels">The levels.</param>
    public static DirectoryInfo? GetParentDirectoryInfo(this DirectoryInfo? directoryInfo, int levels)
    {
        ArgumentNullException.ThrowIfNull(directoryInfo);

        levels = Math.Abs(levels);
        if (levels == 0) return directoryInfo;

        var parentDirectoryInfo = directoryInfo.Parent;
        if (parentDirectoryInfo == null) return directoryInfo;

        --levels;

        return levels >= 1 ? parentDirectoryInfo.GetParentDirectoryInfo(levels) : parentDirectoryInfo;
    }

    /// <summary>
    /// Combines path and root based
    /// on the current value of <see cref="Path.DirectorySeparatorChar"/>
    /// of the current OS or passes through a drive-letter rooted path.</summary>
    /// <param name="directoryInfo">The specified <see cref="DirectoryInfo"/>.</param>
    /// <param name="path">The path.</param>
    /// <remarks>
    /// For detail, see https://github.com/BryanWilhite/SonghayCore/issues/14
    /// and <see cref="ProgramFileUtility.GetCombinedPath(string, string)" />.
    /// </remarks>
    public static string ToCombinedPath(this DirectoryInfo? directoryInfo, string? path)
    {
        ArgumentNullException.ThrowIfNull(directoryInfo);

        return ProgramFileUtility.GetCombinedPath(directoryInfo.FullName, path);
    }

    /// <summary>
    /// Verifies the specified <see cref="DirectoryInfo"/>
    /// with conventional error handling.
    /// </summary>
    /// <param name="directoryInfo">The specified <see cref="DirectoryInfo"/>.</param>
    /// <param name="expectedDirectoryName">The expected directory name.</param>
    public static void VerifyDirectory(this DirectoryInfo? directoryInfo, string? expectedDirectoryName)
    {
        if (directoryInfo == null)
            throw new DirectoryNotFoundException("The expected directory is not here.");

        if (!directoryInfo.Exists)
            throw new DirectoryNotFoundException("The expected directory does not exist.");

        if (!directoryInfo.Name.EqualsInvariant(expectedDirectoryName))
            throw new DirectoryNotFoundException(
                $"The expected directory is not here. [actual: {expectedDirectoryName ?? "[name]"}");
    }
}
