using Microsoft.Extensions.Configuration;
using System.IO;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extension of <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class IConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder WithAnyConfigurationConventions(this IConfigurationBuilder builder)
        {
            return builder
                .WithSettingsJsonInCurrentDirectory();
        }

        public static IConfigurationBuilder WithSettingsJsonInCurrentDirectory(this IConfigurationBuilder builder)
        {
            if (builder == null) return null;

            return builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", optional: true, reloadOnChange: true);
        }
    }
}
