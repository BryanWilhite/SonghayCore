// ReSharper disable InconsistentLogPropertyNaming
// ReSharper disable LogMessageIsSentenceProblem

namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="ILogger"/>
/// to assist with “structured logging.”
/// </summary>
/// <remarks>
/// For a video overview of “structured logging,” see https://www.youtube.com/watch?v=NlBjVJPkT6M 🎥
///
/// To prevent passing null instances of <see cref="ILogger"/> into these methods,
/// use <see cref="ILoggerUtility.AsInstanceOrNullLogger"/>.
/// </remarks>
// ReSharper disable once InconsistentNaming
public static class ILoggerExtensions
{
    /// <summary>
    /// Calls <see cref="LoggerExtensions.LogError(ILogger, string?, object?[])" /> for missing data.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="caption">The caption.</param>
    public static void LogErrorForMissingData(this ILogger logger, string caption)
    {
        logger.LogError("The expected {caption} data is not here.", caption);
    }

    /// <summary>
    /// Calls <see cref="LoggerExtensions.LogError(ILogger, string?, object?[])"/> for missing data.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <param name="logger">The logger.</param>
    public static void LogErrorForMissingData<TData>(this ILogger logger)
    {
        var dataType = typeof(TData);

        logger.LogError("The expected {dataType} data is not here.", dataType);
    }

    /// <summary>
    /// Logs <see cref="Environment.NewLine"/> for human readability.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="level">The level.</param>
    public static void LogNewLine(this ILogger logger, LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Trace:
                logger.LogTrace("{newLine}", Environment.NewLine);
                break;
            case LogLevel.Debug:
                logger.LogDebug("{newLine}", Environment.NewLine);
                break;
            case LogLevel.Information:
                logger.LogInformation("{newLine}", Environment.NewLine);
                break;
            case LogLevel.Warning:
                logger.LogWarning("{newLine}", Environment.NewLine);
                break;
            case LogLevel.Error:
                logger.LogError("{newLine}", Environment.NewLine);
                break;
            case LogLevel.Critical:
                logger.LogCritical("{newLine}", Environment.NewLine);
                break;
            case LogLevel.None:
            default:
                break;
        }
    }

    /// <summary>
    /// Calls <see cref="LogNewLine(ILogger, LogLevel)"/>
    /// for <see cref="LogLevel.Information"/>.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public static void LogInformationNewLine(this ILogger logger) => logger.LogNewLine(LogLevel.Information);

    /// <summary>
    /// Traces data type and value.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <param name="logger">The logger.</param>
    /// <param name="data">The data.</param>
    /// <remarks>
    /// For the best results, ensure that the <see cref="object.ToString"/> method is properly overridden.
    /// </remarks>
    public static void LogTraceDataTypeAndValue<TData>(this ILogger logger, TData? data)
    {
        var type = typeof(TData);
        logger.LogTrace("{dataType}: {data}", type.Name, data);
    }

    /// <summary>
    /// Calls <see cref="LogNewLine(ILogger, LogLevel)"/>
    /// for <see cref="LogLevel.Trace"/>.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public static void LogTraceNewLine(this ILogger logger) => logger.LogNewLine(LogLevel.Trace);

    /// <summary>
    /// Conventional trace of the specified method call.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="methodName">Name of the method.</param>
    public static void LogTraceMethodCall(this ILogger logger, string? methodName)
    {
        logger.LogTrace("Calling {methodName}...", methodName);
    }
}
