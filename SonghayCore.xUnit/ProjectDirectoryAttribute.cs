using Songhay.Tests.Extensions;
using Songhay.Tests.Models;

namespace Songhay.Tests;

/// <summary>
/// 
/// </summary>
public class ProjectDirectoryAttribute : DataAttribute
{
    public ProjectDirectoryAttribute(params object[] inlineData)
    {
        _inlineData = inlineData;
    }

    public ProjectDirectoryAttribute(string pathSuffix, ProjectDirectoryOption placement, params object[] inlineData)
    {
        _pathSuffix = pathSuffix;
        _placement = placement;
        _inlineData = inlineData;
    }

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
    private readonly ProjectDirectoryOption? _placement;
    private readonly object[] _inlineData;
}
