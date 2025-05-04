using System.Text.RegularExpressions;
using Songhay.Tests.Extensions;

namespace Songhay.Tests;

/// <summary>File-based data source for a data theory.</summary>
public partial class ProjectFileDataAttribute : DataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectFileDataAttribute"/> class.
    /// </summary>
    /// <param name="relativePaths">The relative paths.</param>
    public ProjectFileDataAttribute(params string[] relativePaths) : this(null, [], relativePaths)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectFileDataAttribute" /> class.
    /// </summary>
    /// <param name="inlineData">The inline data.</param>
    /// <param name="relativePaths">The relative paths.</param>
    /// <remarks>
    /// The order of elements in <c>inlineData</c> must have the order of args.
    /// So <c>new object[] { 1, "two" }</c> must have <c>int one, string two,</c>.
    /// </remarks>
    public ProjectFileDataAttribute(object[] inlineData, params string[] relativePaths) : this(null, inlineData, relativePaths)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectFileDataAttribute"/> class.
    /// </summary>
    /// <param name="typeInAssembly">The type in assembly.</param>
    /// <param name="relativePaths">The relative paths.</param>
    public ProjectFileDataAttribute(Type? typeInAssembly, params string[] relativePaths) : this(typeInAssembly, [], relativePaths)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectFileDataAttribute" /> class.
    /// </summary>
    /// <param name="typeInAssembly">The type in assembly.</param>
    /// <param name="inlineData">The inline data.</param>
    /// <param name="relativePaths">The relative paths.</param>
    /// <remarks>
    /// The order of elements in <c>inlineData</c> must have the order of args.
    /// So <c>new object[] { 1, "two" }</c> must have <c>int one, string two,</c>.
    /// </remarks>
    public ProjectFileDataAttribute(Type? typeInAssembly, object[] inlineData, params string[] relativePaths)
    {
        _typeInAssembly = typeInAssembly;
        _inlineData = inlineData;
        _relativePaths = relativePaths;
        _numbersOfDirectoryLevels = relativePaths.Select(GetNumberOfDirectoryLevels).ToArray();
    }

    /// <summary>Returns the data to be used to test the theory.</summary>
    /// <param name="testMethod">The method that is being tested</param>
    /// <returns>One or more sets of theory data. Each invocation of the test method
    /// is represented by a single object array.</returns>
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {

        var data = new List<object>();

        data.AddRange(GetInlineData());
        data.AddRange(GetDataForFiles(testMethod));

        return [data.ToArray()];
    }

    private object[] GetDataForFiles(MethodInfo testMethod)
    {
        if (_relativePaths.Length == 0) return [];

        var pairs = _relativePaths.Zip(_numbersOfDirectoryLevels, (path, levels) => new KeyValuePair<string, int>(path, levels));

        var infos = pairs.Select(pair =>
        {
            _typeInAssembly ??= testMethod.DeclaringType.ToReferenceTypeValueOrThrow();

            DirectoryInfo projectDirectoryInfo = this.GetAssemblyParentDirectoryInfo(_typeInAssembly, pair.Value);
            string file = projectDirectoryInfo.ToCombinedPath(pair.Key)
                    .Replace("../", string.Empty)
                    .Replace(@"..\", string.Empty)
                    .Replace("./", string.Empty)
                    .Replace(@".\", string.Empty)
                ;

            return new FileInfo(file);
        });

        return infos.OfType<object>().ToArray();
    }

    private object[] GetInlineData() => _inlineData.Length == 0 ? [] : _inlineData;

    private static int GetNumberOfDirectoryLevels(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return 0;

        MatchCollection matches = DirectoryLevelsRegex().Matches(path);

        return matches.Count;
    }

    private Type? _typeInAssembly;
    private readonly object[] _inlineData;
    private readonly string[] _relativePaths;
    private readonly int[] _numbersOfDirectoryLevels;

    [GeneratedRegex(@"\.\./|\.\.\\")]
    private static partial Regex DirectoryLevelsRegex();
}
