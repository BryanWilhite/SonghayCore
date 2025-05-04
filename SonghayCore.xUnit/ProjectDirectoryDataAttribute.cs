using Songhay.Tests.Extensions;
using Songhay.Tests.Models;

namespace Songhay.Tests;

/// <summary>
/// Defines a custom <see cref="DataAttribute"/>
/// for loading the <see cref="DirectoryInfo"/> test-method argument,
/// representing a directory within the test project.
/// </summary>
public class ProjectDirectoryDataAttribute : DataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDirectoryDataAttribute"/> class.
    /// </summary>
    /// <param name="inlineData">The inline data of <see cref="InlineDataAttribute"/> conventions.</param>
    public ProjectDirectoryDataAttribute(params object[] inlineData) : this(null, ProjectDirectoryOption.AppendNothing, inlineData)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDirectoryDataAttribute"/> class.
    /// </summary>
    /// <param name="pathSuffix">the directory path to append</param>
    /// <param name="placement">the <see cref="ProjectDirectoryOption"/></param>
    /// <param name="inlineData">The inline data of <see cref="InlineDataAttribute"/> conventions.</param>
    public ProjectDirectoryDataAttribute(string? pathSuffix, ProjectDirectoryOption placement, params object[] inlineData)
    {
        _pathSuffix = pathSuffix;
        _placement = placement;
        _inlineData = inlineData;
    }

    /// <summary>Returns the data to be used to test the theory.</summary>
    /// <param name="testMethod">The method that is being tested</param>
    /// <returns>One or more sets of theory data. Each invocation of the test method
    /// is represented by a single object array.</returns>
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        Type methodType = testMethod.DeclaringType.ToReferenceTypeValueOrThrow();

        var projectInfo = methodType.Assembly.GetNetCoreProjectDirectoryInfo();

        var data = new List<object>
        {
            GetDirectoryInfo(projectInfo, methodType.Name, testMethod.Name).ToReferenceTypeValueOrThrow()
        };

        data.AddRange(_inlineData);

        return [data.ToArray()];
    }

    private DirectoryInfo? GetDirectoryInfo(DirectoryInfo projectInfo, string testTypeName, string testMethodName)
    {
        return _placement switch
        {
            ProjectDirectoryOption.AppendPathSuffixAndTestTypeName =>
                string.IsNullOrWhiteSpace(_pathSuffix) ?
                    projectInfo.FindDirectory(testTypeName)
                    :
                    new DirectoryInfo(projectInfo.ToCombinedPath(_pathSuffix)).FindDirectory(testTypeName),

            ProjectDirectoryOption.AppendPathSuffixOnly =>
                string.IsNullOrWhiteSpace(_pathSuffix) ?
                    projectInfo.FindDirectory(testTypeName)
                    :
                    new DirectoryInfo(projectInfo.ToCombinedPath(_pathSuffix)),

            ProjectDirectoryOption.AppendPathSuffixTestTypeNameAndTestMethodName =>
                string.IsNullOrWhiteSpace(_pathSuffix) ?
                    projectInfo.FindDirectory(testTypeName)
                    :
                    new DirectoryInfo(projectInfo.ToCombinedPath(_pathSuffix)).FindDirectory(testTypeName).FindDirectory(testMethodName),

            ProjectDirectoryOption.AppendTestTypeNameAndPathSuffix =>
                string.IsNullOrWhiteSpace(_pathSuffix) ?
                    projectInfo.FindDirectory(testTypeName)
                    :
                    new DirectoryInfo(projectInfo.FindDirectory(testTypeName).ToCombinedPath(_pathSuffix)),

            ProjectDirectoryOption.AppendTestTypeNameAndTestMethodName =>
                projectInfo.FindDirectory(testTypeName).FindDirectory(testMethodName),

            ProjectDirectoryOption.AppendTestTypeNameOnly =>
                projectInfo.FindDirectory(testTypeName),

            ProjectDirectoryOption.AppendTestTypeNameTestMethodNameAndPathSuffix =>
                new DirectoryInfo(projectInfo.FindDirectory(testTypeName).FindDirectory(testMethodName).ToCombinedPath(_pathSuffix)),

            _ => projectInfo
        };
    }

    private readonly string? _pathSuffix;
    private readonly ProjectDirectoryOption _placement;
    private readonly object[] _inlineData;
}
