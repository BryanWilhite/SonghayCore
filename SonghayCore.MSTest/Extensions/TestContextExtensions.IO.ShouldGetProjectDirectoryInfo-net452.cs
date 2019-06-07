#if NET452 || NET462

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="TestContext"/>.
    /// </summary>
    public static partial class TestContextExtensions
    {
        /// <summary>
        /// Should get project directory information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static DirectoryInfo ShouldGetProjectDirectoryInfo(this TestContext context, Type type)
        {
            var projectDirectoryInfo = context
                .ShouldGetAssemblyDirectoryInfo(type) // Debug or Release
                ?.Parent // bin
                ?.Parent;
            context.ShouldFindDirectory(projectDirectoryInfo?.FullName);

            return projectDirectoryInfo;
        }
    }
}

#endif
