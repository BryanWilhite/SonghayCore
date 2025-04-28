namespace Songhay;

/// <summary>
/// Centralized routines for <see cref="ILogger"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ILoggerUtility
{
    /// <summary>
    /// When the specified <see cref="ILogger"/> is null,
    /// fallback to Microsoft’s <see cref="NullLogger"/>.
    /// </summary>
    /// <param name="logger">the specified <see cref="ILogger"/></param>
    public static ILogger AsInstanceOrNullLogger([NotNull]ILogger? logger)
    {
        if (logger == null) logger = NullLogger.Instance;

        return  logger;
    }


    /// <summary>
    /// When the specified <see cref="ILogger{T}"/> is null,
    /// fallback to Microsoft’s <see cref="NullLogger{T}"/>.
    /// </summary>
    /// <typeparam name="TCategory">logging category by type</typeparam>
    /// <param name="logger">the specified <see cref="ILogger"/></param>
    public static ILogger<TCategory> AsInstanceOrNullLogger<TCategory>([NotNull]ILogger<TCategory>? logger)
    {
        if (logger == null) logger = NullLogger<TCategory>.Instance;

        return logger;
    }
}
