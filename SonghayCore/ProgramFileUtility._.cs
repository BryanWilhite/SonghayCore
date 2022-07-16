namespace Songhay;

/// <summary>
/// A few static helper members for <see cref="System.IO"/>.
/// </summary>
public static partial class ProgramFileUtility
{
    static ProgramFileUtility()
    {
        Backslash = '\\';
        ForwardSlash = '/';
        IsForwardSlashSystemField = Path.DirectorySeparatorChar.Equals(ForwardSlash);
        TraceSource = TraceSources.Instance.GetConfiguredTraceSource();
    }

    static readonly TraceSource? TraceSource;

    /// <summary>
    /// Counts the parent directory chars.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <remarks>
    /// This method is useful when running <see cref="GetParentDirectory(string, int)"/>.
    /// 
    /// WARNING: call <see cref="NormalizePath(string)"/> to prevent incorrectly returning <c>0</c>
    /// in cross-platform scenarios.
    /// </remarks>
    public static int CountParentDirectoryChars(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return default;

        var parentDirectoryCharsPattern = $@"\.\.\{Path.DirectorySeparatorChar}";
        var matches = Regex.Matches(path, parentDirectoryCharsPattern);

        return matches.Count;
    }

    /// <summary>
    /// Finds the parent directory.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="parentName">Name of the parent.</param>
    /// <param name="levels">The levels.</param>
    public static string? FindParentDirectory(string? path, string? parentName, int levels) =>
        FindParentDirectoryInfo(path, parentName, levels)?.FullName;

    /// <summary>
    /// Finds the parent <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="parentName">Name of the parent.</param>
    /// <param name="levels">The levels.</param>
    public static DirectoryInfo? FindParentDirectoryInfo(string? path, string? parentName, int levels)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new DirectoryNotFoundException("The expected directory is not here.");

        var info = new DirectoryInfo(path);

        var isParent = (info.Name == parentName);
        var hasNullParent = (info.Parent == null);
        var hasTargetParent = !hasNullParent && (info.Parent?.Name == parentName);

        if (!info.Exists) return null;
        if (isParent) return info;
        if (hasNullParent) return null;
        if (hasTargetParent) return info.Parent;

        levels = Math.Abs(levels);
        --levels;

        var hasNoMoreLevels = (levels == 0);

        return hasNoMoreLevels ? null : FindParentDirectoryInfo(info.Parent?.FullName, parentName, levels);
    }

    /// <summary>Combines path and root based
    /// on the ambient value of <see cref="Path.DirectorySeparatorChar"/>
    /// of the current OS.</summary>
    /// <param name="root">The root.</param>
    /// <param name="path">The path.</param>
    /// <remarks>
    /// For detail, see:
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/14
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/32
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/97
    /// </remarks>
    public static string GetCombinedPath(string? root, string? path)
    {
        root.ThrowWhenNullOrWhiteSpace();
        path.ThrowWhenNullOrWhiteSpace();

        path = GetRelativePath(path);

        return Path.IsPathRooted(path)
            ? path
            : Path.Combine(NormalizePath(root)!, path!);
    }

    /// <summary>Combines path and root based
    /// on the ambient value of <see cref="Path.DirectorySeparatorChar"/>
    /// of the current OS.</summary>
    /// <param name="root">The root.</param>
    /// <param name="path">The path.</param>
    /// <param name="fileIsExpected">
    /// when <c>true</c> will throw <see cref="FileNotFoundException"/>
    /// when combined path is not of a file; otherwise
    /// will throw <see cref="DirectoryNotFoundException"/>
    /// when combined path is not a directory
    /// </param>
    /// <remarks>
    /// For detail, see:
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/14
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/32
    /// 📚 https://github.com/BryanWilhite/SonghayCore/issues/97
    /// </remarks>
    public static string GetCombinedPath(string? root, string? path, bool fileIsExpected)
    {
        var combinedPath = GetCombinedPath(root, path);

        if (fileIsExpected)
        {
            if (!File.Exists(combinedPath))
                throw new FileNotFoundException($"The expected file, `{combinedPath}`, is not here.");
        }
        else
        {
            if (!Directory.Exists(combinedPath))
                throw new DirectoryNotFoundException($"The expected directory, `{combinedPath}`, is not here.");
        }

        return combinedPath;
    }

    /// <summary>
    /// Gets the parent directory.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="levels">The levels.</param>
    /// <remarks>
    /// A recursive wrapper for <see cref="Directory.GetParent(string)"/>.
    /// </remarks>
    public static string? GetParentDirectory(string? path, int levels)
    {
        path.ThrowWhenNullOrWhiteSpace();

        levels = Math.Abs(levels);
        if (levels == 0) return path;

        var info = Directory.GetParent(path);
        if (info == null) return path;
        path = info.FullName;

        --levels;

        return levels >= 1 ? GetParentDirectory(path, levels) : path;
    }

    /// <summary>
    /// Gets the parent <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="levels">The levels.</param>
    /// <remarks>
    /// A recursive wrapper for <see cref="Directory.GetParent(string)"/>.
    /// </remarks>
    public static DirectoryInfo? GetParentDirectoryInfo(string? path, int levels)
    {
        path.ThrowWhenNullOrWhiteSpace();

        var info = new DirectoryInfo(path);

        levels = Math.Abs(levels);
        if (levels == 0) return info;

        if (info.Parent == null) return info;
        path = info.Parent.FullName;

        --levels;

        return (levels >= 1) ? GetParentDirectoryInfo(path, levels) : info.Parent;
    }

    /// <summary>
    /// Gets the relative path from the specified file segment
    /// without leading dots (<c>.</c>) or <see cref="Path.DirectorySeparatorChar" /> chars.
    /// </summary>
    /// <param name="fileSegment">The file segment.</param>
    /// <remarks>
    /// This method is the equivalent of calling:
    ///  * <see cref="TrimLeadingDirectorySeparatorChars(string)"/>
    ///  * <see cref="NormalizePath(string)"/>
    ///  * <see cref="RemoveBackslashPrefixes(string)"/>
    ///  * <see cref="RemoveForwardslashPrefixes(string)"/>
    /// </remarks>
    public static string? GetRelativePath(string? fileSegment)
    {
        fileSegment.ThrowWhenNullOrWhiteSpace();

        fileSegment = TrimLeadingDirectorySeparatorChars(fileSegment);
        fileSegment = NormalizePath(fileSegment);
        fileSegment = RemoveBackslashPrefixes(fileSegment);
        fileSegment = RemoveForwardslashPrefixes(fileSegment);

        return fileSegment;
    }

    /// <summary>
    /// Returns <c>true</c> when the current OS
    /// uses forward-slash (<c>/</c>) paths or not.
    /// </summary>
    public static bool IsForwardSlashSystem() => IsForwardSlashSystemField;

    /// <summary>
    /// Normalizes the specified path with respect
    /// to the ambient value of <see cref="Path.DirectorySeparatorChar"/>.
    /// </summary>
    /// <param name="path">The path.</param>
    public static string? NormalizePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;

        return IsForwardSlashSystemField
            ? path.Replace(Backslash, ForwardSlash)
            : path.Replace(ForwardSlash, Backslash);
    }

    /// <summary>
    /// Removes conventional Directory prefixes
    /// for relative paths, e.g. <c>..\</c> or <c>.\</c>
    /// </summary>
    /// <param name="path">The path.</param>
    public static string? RemoveBackslashPrefixes(string? path) =>
        path?
            .TrimStart(Backslash)
            .Replace($"..{Backslash}", string.Empty)
            .Replace($".{Backslash}", string.Empty);

    /// <summary>
    /// Removes conventional Directory prefixes
    /// for relative paths based on the ambient value\
    /// of <see cref="Path.DirectorySeparatorChar"/>.
    /// </summary>
    /// <param name="path">The path.</param>
    public static string? RemoveConventionalPrefixes(string? path) =>
        IsForwardSlashSystemField
            ? RemoveForwardslashPrefixes(path)
            : RemoveBackslashPrefixes(path);

    /// <summary>
    /// Removes conventional Directory prefixes
    /// for relative paths, e.g. <c>../</c> or <c>./</c>.
    /// </summary>
    /// <param name="path">The path.</param>
    public static string? RemoveForwardslashPrefixes(string? path) =>
        path?
            .TrimStart(ForwardSlash)
            .Replace($"..{ForwardSlash}", string.Empty)
            .Replace($".{ForwardSlash}", string.Empty);

    /// <summary>
    /// Trims the leading directory separator chars.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <remarks>
    /// Trims leading <see cref="Path.AltDirectorySeparatorChar"/> and/or <see cref="Path.DirectorySeparatorChar"/>,
    /// formatting relative paths for <see cref="Path.Combine(string, string)"/>.
    /// </remarks>
    public static string? TrimLeadingDirectorySeparatorChars(string? path) =>
        string.IsNullOrWhiteSpace(path) ? path : path.TrimStart(Backslash, ForwardSlash);

    static readonly bool IsForwardSlashSystemField;
    static readonly char Backslash;
    static readonly char ForwardSlash;
}
