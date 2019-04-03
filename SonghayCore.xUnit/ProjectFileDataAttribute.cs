using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Songhay.Extensions;
using Songhay.Test.Extensions;
using Xunit;
using Xunit.Sdk;

namespace SonghayCore.xUnit
{
    /// <summary>File-based data source for a data theory.</summary>
    public class ProjectFileDataAttribute : DataAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectFileDataAttribute"/> class.
        /// </summary>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <param name="relativePaths">The relative paths.</param>
        public ProjectFileDataAttribute(Type typeInAssembly, params string[] relativePaths)
        {
            _typeInAssembly = typeInAssembly;
            _relativePaths = relativePaths;
            _numbersOfDirectoryLevels = relativePaths.Select(GetNumberOfDirectoryLevels).ToArray();
        }

        /// <summary>Initializes a new instance of the <see cref="ProjectFileDataAttribute"/> class.</summary>
        /// <param name="typeInAssembly">The type in assembly to find the project directory.</param>
        /// <param name="relativePath">The path relative to the project directory.</param>
        /// <param name="numberOfDirectoryLevels">The number of directory levels above the assembly directory to the project directory.</param>
        /// <remarks>
        /// For the modern .NET Core project, <c>numberOfDirectoryLevels = 3</c>: [netcoreapp* or net*]/[Debug or Release]/bin/[project folder]
        /// For the legacy .NET project, <c>numberOfDirectoryLevels = 2</c>: [Debug or Release]/bin/[project folder]
        /// </remarks>
        [Obsolete(@"instead of `numberOfDirectoryLevels` use `..\\..\\` or `../../` prefixes instead")]
        public ProjectFileDataAttribute(Type typeInAssembly, string relativePath, int numberOfDirectoryLevels)
        {
            _typeInAssembly = typeInAssembly;
            _relativePaths = new[] { relativePath };
            _numbersOfDirectoryLevels = new[] { numberOfDirectoryLevels };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectFileDataAttribute"/> class.
        /// </summary>
        /// <param name="typeInAssembly">The type in assembly.</param>
        /// <param name="relativePaths">The relative paths.</param>
        /// <param name="numberOfDirectoryLevels">The number of directory levels.</param>
        [Obsolete(@"instead of `numberOfDirectoryLevels` use `..\\..\\` or `../../` prefixes instead")]
        public ProjectFileDataAttribute(Type typeInAssembly, string[] relativePaths, int numberOfDirectoryLevels)
        {
            _typeInAssembly = typeInAssembly;
            _relativePaths = relativePaths;
            _numbersOfDirectoryLevels = new[] { numberOfDirectoryLevels };
        }

        /// <summary>Returns the data to be used to test the theory.</summary>
        /// <param name="testMethod">The method that is being tested</param>
        /// <returns>One or more sets of theory data. Each invocation of the test method
        /// is represented by a single object array.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {

            object[] data = null;

            if ((_relativePaths != null) && _relativePaths.Any()) data = GetDataForFiles();

            return new[] { data };
        }

        private object[] GetDataForFiles()
        {
            var pairs = _relativePaths.Zip(_numbersOfDirectoryLevels, (path, levels) => new KeyValuePair<string, int>(path, levels));

            var infos = pairs.Select(pair =>
            {
                var project_directory_info = this.GetAssemblyParentDirectoryInfo(_typeInAssembly, pair.Value);
                var file = project_directory_info.ToCombinedPath(pair.Key)
                        .Replace("../", string.Empty)
                        .Replace(@"..\", string.Empty)
                        .Replace("./", string.Empty)
                        .Replace(@".\", string.Empty)
                    ;
                return new FileInfo(file);
            });

            return infos.OfType<object>().ToArray();
        }

        private static int GetNumberOfDirectoryLevels(string path)
        {
            if (string.IsNullOrEmpty(path)) return 0;
            var relative_parent_path_matches = Regex.Matches(path, @"\.\./|\.\.\\");
            return relative_parent_path_matches.Count;
        }

        private readonly Type _typeInAssembly;
        private readonly string[] _relativePaths;
        private readonly int[] _numbersOfDirectoryLevels;
}
