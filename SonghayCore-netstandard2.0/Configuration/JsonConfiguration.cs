using Microsoft.Extensions.Configuration;
using System.IO;

namespace Songhay.Configuration
{
    /// <summary>
    /// Defines the conventional JSON configuration for .NET Core.
    /// </summary>
    /// <remarks>
    /// This defintion is expecting to see <c>app-settings.json</c>
    /// next to the executable.
    ///
    /// For background, see “.NET Core Configuration”
    /// [https://github.com/BryanWilhite/dotnet-core/tree/master/dotnet-console-configuration]
    /// </remarks>
    public class JsonConfiguration
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="JsonConfiguration"/> class from being created.
        /// </summary>
        private JsonConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the <see cref="IConfigurationRoot"/>.
        /// </summary>
        /// <value>
        /// The <see cref="IConfigurationRoot"/>.
        /// </value>
        public IConfigurationRoot Configuration { get; private set; }

        /// <summary>
        /// Gets the singleton instance
        /// of <see cref="JsonConfiguration"/>.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static JsonConfiguration Instance { get { return Nested.instance; } }

        static readonly JsonConfiguration configuration;

        static class Nested
        {
            static Nested() { }

            internal static readonly JsonConfiguration instance = new JsonConfiguration();
        }
    }
}
