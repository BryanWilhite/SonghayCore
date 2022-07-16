namespace Songhay;

/// <summary>
/// Defines shared routines for Studio programs.
/// </summary>
public static partial class ProgramUtility
{
    /// <summary>
    /// Initializes the trace source.
    /// </summary>
    /// <param name="listener">The listener.</param>
    public static TraceSource? InitializeTraceSource(TraceListener? listener)
    {
        var traceSource = TraceSources
            .Instance
            .GetTraceSourceFromConfiguredName()
            .WithSourceLevels();

        if (listener != null) traceSource?.Listeners.Add(listener);

        return traceSource;
    }

    /// <summary>
    /// Loads the configuration.
    /// </summary>
    /// <param name="basePath">The base path.</param>
    public static IConfigurationRoot LoadConfiguration(string? basePath) =>
        LoadConfiguration(basePath, builderModifier: null);

    /// <summary>
    /// Loads the built configuration.
    /// </summary>
    /// <param name="basePath">The base path.</param>
    /// <param name="requiredJsonConfigurationFiles">specify any additional JSON configuration files before build</param>
    /// <returns>Returns the built configuration.</returns>
    public static IConfigurationRoot
        LoadConfiguration(string? basePath, params string[] requiredJsonConfigurationFiles) =>
        LoadConfiguration(basePath, builderModifier: null, requiredJsonConfigurationFiles);

    /// <summary>
    /// Loads the built configuration.
    /// </summary>
    /// <param name="basePath">The base path.</param>
    /// <param name="builderModifier">Allows modification of <see cref="ConfigurationBuilder"/> before build.</param>
    /// <param name="requiredJsonConfigurationFiles">specify any additional JSON configuration files before build</param>
    /// <returns>Returns the built configuration.</returns>
    public static IConfigurationRoot LoadConfiguration(string? basePath,
        Func<IConfigurationBuilder, IConfigurationBuilder>? builderModifier,
        params string[] requiredJsonConfigurationFiles)
    {
        Console.WriteLine("Loading configuration...");

        var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(basePath)
                .AddJsonFile("./appsettings.json", optional: false, reloadOnChange: false)
            ;

        foreach (var jsonFile in requiredJsonConfigurationFiles)
        {
            builder.AddJsonFile(jsonFile, optional: false, reloadOnChange: false);
        }

        if (builderModifier != null) builder = builderModifier(builder);

        Console.WriteLine("Building configuration...");

        var configuration = builder.Build();

        return configuration;
    }

    /// <summary>
    /// Pauses the shell Program in <c>DEBUG</c> mode.
    /// </summary>
    public static void HandleDebug()
    {
#if DEBUG
        Console.WriteLine($"{Environment.NewLine}Press any key to continue...");
        Console.ReadKey(false);
#endif
    }
}
