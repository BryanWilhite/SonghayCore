namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ILoggerProvider"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ILoggerProviderExtensions
{
    /// <summary>
    /// Generates <see cref="ILogger{TCategory}"/>
    /// from the specified <see cref="ILoggerProvider"/>.
    /// </summary>
    /// <param name="provider">the <see cref="ILoggerProvider"/></param>
    /// <typeparam name="TCategory">the logging category</typeparam>
    public static ILogger<TCategory>? GenerateLogger<TCategory>(this ILoggerProvider? provider)
    {
        if (provider == null) return null;

        ILogger<TCategory> logger = LoggerFactory.Create(builder => builder.AddProvider(provider)).CreateLogger<TCategory>();

        return logger;
    }
}
