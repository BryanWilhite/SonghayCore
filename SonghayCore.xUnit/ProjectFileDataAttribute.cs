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
        /// <summary>Initializes a new instance of the <see cref="ProjectFileDataAttribute"/> class.</summary>
        /// <param name="typeInAssembly">The type in assembly to find the project directory.</param>
        /// <param name="relativePath">The path relative to the project directory.</param>
        /// <param name="numberOfDirectoryLevels">The number of directory levels above the assembly directory to the project directory.</param>
        /// <remarks>
        /// For the modern .NET Core project, <c>numberOfDirectoryLevels = 3</c>: [netcoreapp* or net*]/[Debug or Release]/bin/[project folder]
        /// For the legacy .NET project, <c>numberOfDirectoryLevels = 2</c>: [Debug or Release]/bin/[project folder]
        /// </remarks>
        public ProjectFileDataAttribute(Type typeInAssembly, string relativePath, int numberOfDirectoryLevels)
        {
            _typeInAssembly = typeInAssembly;
            _relativePath = relativePath;
            _numberOfDirectoryLevels = numberOfDirectoryLevels;
        }

        public ProjectFileDataAttribute(Type typeInAssembly, string[] relativePaths, int numberOfDirectoryLevels)
        {
            _typeInAssembly = typeInAssembly;
            _relativePaths = relativePaths;
            _numberOfDirectoryLevels = numberOfDirectoryLevels;
        }

        /// <summary>Returns the data to be used to test the theory.</summary>
        /// <param name="testMethod">The method that is being tested</param>
        /// <returns>One or more sets of theory data. Each invocation of the test method
        /// is represented by a single object array.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var projectDirectoryInfo = this.GetAssemblyParentDirectoryInfo(_typeInAssembly, _numberOfDirectoryLevels);

            object[] data = null;

            if (!string.IsNullOrEmpty(_relativePath)) data = GetDataForFile(projectDirectoryInfo);
            if ((_relativePaths != null) && _relativePaths.Any()) data = GetDataForFiles(projectDirectoryInfo);

            return new[] { data };
        }

        private object[] GetDataForFile(DirectoryInfo projectDirectoryInfo)
        {
            var file = projectDirectoryInfo.ToCombinedPath(_relativePath);

            Assert.True(File.Exists(file), "The expected file in project is not here.");

            return new object[] { File.ReadAllText(file) };
        }

        private object[] GetDataForFiles(DirectoryInfo projectDirectoryInfo)
        {
            var infos = new List<FileInfo>();

            foreach (var relativePathToFile in _relativePaths)
            {
                var file = projectDirectoryInfo.ToCombinedPath(relativePathToFile);
                Assert.True(File.Exists(file), "The expected file in project is not here.");
                infos.Add(new FileInfo(file));
            }

            return infos.OfType<object>().ToArray();
        }

        private readonly Type _typeInAssembly;
        private readonly string _relativePath;
        private readonly string[] _relativePaths;
        private readonly int _numberOfDirectoryLevels;
    }
}
