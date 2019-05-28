using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Songhay.Extensions;
using Songhay.Tests.Extensions;
using Xunit.Sdk;

namespace Songhay.Tests
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
        public ProjectFileDataAttribute(Type typeInAssembly, object[] inlineData, params string[] relativePaths)
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
            data.AddRange(GetDataForFiles());

            return new[] { data.ToArray() };
        }

        IEnumerable<object> GetDataForFiles()
        {
            if ((_relativePaths == null) || !_relativePaths.Any()) return Enumerable.Empty<object>().ToArray();

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

        IEnumerable<object> GetInlineData()
        {
            if ((_inlineData == null) || !_inlineData.Any()) return Enumerable.Empty<object>().ToArray();
            return _inlineData;
        }

        static int GetNumberOfDirectoryLevels(string path)
        {
            if (string.IsNullOrEmpty(path)) return 0;
            var relative_parent_path_matches = Regex.Matches(path, @"\.\./|\.\.\\");
            return relative_parent_path_matches.Count;
        }

        readonly Type _typeInAssembly;
        readonly object[] _inlineData;
        readonly string[] _relativePaths;
        readonly int[] _numbersOfDirectoryLevels;
    }
}
