using System;

namespace Songhay.Models;

/// <summary>
/// Centralizes conventional values used for DI.
/// </summary>
public static class DependencyInjectionScalars
{
    /// <summary>
    /// The name of the conventional environment variable name for app configuration
    /// </summary>
    public const string AppConfigurationVariableName = "SONGHAY_APP_CONFIGURATION";

    /// <summary>
    /// The default REST API key used for DI
    /// </summary>
    public const string DefaultApiEndPointKey = "default";
}
