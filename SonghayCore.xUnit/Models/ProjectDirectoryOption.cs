namespace Songhay.Tests.Models;

/// <summary>
/// Enumerates the appending strategies
/// for generating the project <see cref="DirectoryInfo"/>
/// of <see cref="ProjectDirectoryDataAttribute"/>.
/// </summary>
public enum ProjectDirectoryOption
{
    /// <summary>
    /// append nothing for generating the project <see cref="DirectoryInfo"/>
    /// </summary>
    AppendNothing,

    /// <summary>
    /// append <c>pathSuffix</c> + <c>methodType.Name</c> for generating the project <see cref="DirectoryInfo"/>
    /// </summary>
    AppendPathSuffixAndTestTypeName,

    /// <summary>
    /// append <c>pathSuffix</c> only for generating the project <see cref="DirectoryInfo"/>
    /// </summary>
    AppendPathSuffixOnly,

    /// <summary>
    /// append <c>pathSuffix</c> + <c>methodType.Name</c> + <c>testMethod.Name</c> for generating the project <see cref="DirectoryInfo"/>
    /// </summary>
    AppendPathSuffixTestTypeNameAndTestMethodName,

    /// <summary>
    /// append <c>methodType.Name</c> + <c>pathSuffix</c> for generating the project <see cref="DirectoryInfo"/>
    /// </summary>
    AppendTestTypeNameAndPathSuffix,

    /// <summary>
    /// append <c>methodType.Name</c> + <c>testMethod.Name</c> for generating the project <see cref="DirectoryInfo"/>
    /// </summary>
    AppendTestTypeNameAndTestMethodName,

    /// <summary>
    /// append <c>methodType.Name</c> only for generating the project <see cref="DirectoryInfo"/>
    /// </summary>
    AppendTestTypeNameOnly,

    /// <summary>
    /// append <c>methodType.Name</c> + <c>testMethod.Name</c> + <c>pathSuffix</c> for generating the project <see cref="DirectoryInfo"/>
    /// </summary>
    AppendTestTypeNameTestMethodNameAndPathSuffix,
}
