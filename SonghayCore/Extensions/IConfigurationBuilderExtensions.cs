namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="IConfigurationBuilder"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IConfigurationBuilderExtensions
{
    /// <summary>
    /// Returns the specified <see cref="IConfigurationBuilder"/>
    /// after calling <see cref="JsonConfigurationExtensions.AddJsonFile(Microsoft.Extensions.Configuration.IConfigurationBuilder,string)"/>
    /// with the conventional environment variable, <c>SONGHAY_APP_SETTINGS_PATH</c>.
    /// </summary>
    /// <param name="configurationBuilder">the <see cref="IConfigurationBuilder"/></param>
    public static IConfigurationBuilder AddConventionalJsonFile(this IConfigurationBuilder? configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        string? path = Environment.GetEnvironmentVariable("SONGHAY_APP_SETTINGS_PATH");
        path.ThrowWhenNullOrWhiteSpace();

        return configurationBuilder.AddJsonFile(path);
    }
}