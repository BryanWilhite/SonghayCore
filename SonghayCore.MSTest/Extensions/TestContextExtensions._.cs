#if NETSTANDARD2_0

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="TestContext"/>.
    /// </summary>
    public static partial class TestContextExtensions
    {
        /// <summary>
        /// Should load configuration from the specified base path.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="basePath">The base path.</param>
        /// <param name="passThroughBuilder">The pass through builder.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method will call <see cref="IConfigurationBuilder.SetBasePath"/>
        /// and <see cref="IConfigurationBuilder.AddJsonFile"/>
        /// for the conventional <c>appsettings.json</c> file.
        /// </remarks>
        public static IConfigurationRoot ShouldLoadConfiguration(this TestContext context, string basePath, Func<IConfigurationBuilder, IConfigurationBuilder> passThroughBuilder)
        {
            context.ShouldFindDirectory(basePath);

            context.WriteLine("Loading configuration...");
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("./appsettings.json", optional: false, reloadOnChange: false)
                ;

            if (passThroughBuilder != null) builder = passThroughBuilder(builder);

            context.WriteLine("Building configuration...");
            var configuration = builder.Build();

            Assert.IsNotNull(configuration, "The expected configuration is not here.");

            return configuration;
        }

        /// <summary>
        /// Should load configuration from conventional project.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="type">The type.</param>
        /// <param name="passThroughBuilder">The pass through builder.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">type - The expected test-class Type is not here.</exception>
        public static IConfigurationRoot ShouldLoadConfigurationFromConventionalProject(this TestContext context, Type type, Func<IConfigurationBuilder, IConfigurationBuilder> passThroughBuilder)
        {
            var directoryInfo = context.ShouldGetConventionalProjectDirectoryInfo(type);
            var configuration = context.ShouldLoadConfiguration(directoryInfo.FullName, passThroughBuilder);
            return configuration;
        }
    }
}

#endif
