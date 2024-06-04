using Songhay.Tests.Models;

namespace Songhay.Tests.Extensions;

/// <summary>
/// Extensions of <see cref="Assembly"/>
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Returns the sibling <see cref="DirectoryInfo"/>
    /// of the directory referenced by <see cref="GetNetCoreProjectDirectoryInfo"/>.
    /// </summary>
    /// <param name="assembly">the <see cref="Assembly"/></param>
    /// <param name="expectedDirectoryName">the name or pattern of the sibling directory (e.g. <c>*.Web</c>)</param>
    public static DirectoryInfo? FindNetCoreProjectDirectorySiblingInfo(this Assembly assembly, string expectedDirectoryName) =>
        assembly.GetNetCoreProjectDirectoryInfo().Parent.FindDirectory(expectedDirectoryName);

    /// <summary>
    /// Returns the sibling <see cref="DirectoryInfo"/>
    /// of the conventional Web project directory
    /// by asserting that <see cref="FindNetCoreProjectDirectorySiblingInfo"/>(<c>*.Web</c>) is valid
    /// with <see cref="Assert.NotNull"/>.
    /// </summary>
    /// <param name="assembly">the <see cref="Assembly"/></param>
    public static DirectoryInfo GetDirectoryInfoOfConventionalWebProjectWithAssertion(this Assembly assembly)
    {
        DirectoryInfo? siblingDirectoryInfo = assembly.FindNetCoreProjectDirectorySiblingInfo("*.Web");

        Assert.NotNull(siblingDirectoryInfo);

        return siblingDirectoryInfo;
    }

    /// <summary>
    /// Returns <see cref="DirectoryInfo"/> based on the specified file segment
    /// and <see cref="ProgramAssemblyUtility.GetPathFromAssembly(System.Reflection.Assembly?, System.String)"/>
    /// by asserting that <see cref="DirectoryInfo.Exists"/> is <c>true</c>
    /// with <see cref="Assert.True(bool)"/>.
    /// </summary>
    /// <param name="assembly">the <see cref="Assembly"/></param>
    /// <param name="fileSegment">the file segment, relative to the <see cref="Assembly"/></param>
    public static DirectoryInfo GetDirectoryInfoWithAssertion(this Assembly assembly, string? fileSegment)
    {
        DirectoryInfo info = new (ProgramAssemblyUtility.GetPathFromAssembly(assembly, fileSegment));

        Assert.True(info.Exists, $"The expected path, `{info.FullName}`, does not exist ({nameof(fileSegment)}: `{fileSegment}`).");

        return info;
    }

    /// <summary>
    /// Returns the path of the conventional, .NET-Solution, project directory
    /// with the .NET-Core file segment (<see cref="TestScalars.NetCoreRelativePathToProjectFolder"/>).
    /// </summary>
    /// <param name="assembly">the <see cref="Assembly"/></param>
    public static DirectoryInfo GetNetCoreProjectDirectoryInfo(this Assembly assembly) =>
        assembly.GetDirectoryInfoWithAssertion(TestScalars.NetCoreRelativePathToProjectFolder);
}
