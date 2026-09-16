using Songhay.SerializerContexts;

namespace Songhay;

/// <summary>
/// Shared routines for <see cref="ProgramMetadata"/>
/// </summary>
/// <remarks>
/// This is the minimalist alternative to <see cref="IConfigurationBuilderExtensions.AddConventionalJsonFile"/>.
/// </remarks>
public static class ProgramMetadataUtility
{
    /// <summary>
    /// Returns an instance of <see cref="ProgramMetadata"/>
    /// based on the presence of one of two conventional environment variables.
    /// </summary>
    public static ProgramMetadata? GetProgramMetadataFromEnvironment()
    {
        string? json = GetJsonForProgramMetadataFromEnvironment();

        if (string.IsNullOrWhiteSpace(json)) return null;

        return JsonSerializer
            .Deserialize<ProgramMetadata>(json, GetJsonDeserializerOptions());
    }

    /// <summary>
    /// Returns <see cref="JsonSerializerOptions"/>
    /// for <see cref="ProgramMetadata"/>
    /// with explicit serializer context(s)
    /// for AOT compiled apps.
    /// </summary>
    public static JsonSerializerOptions GetJsonDeserializerOptions()
    {
        JsonSerializerOptions options = new();

        options.TypeInfoResolverChain.Add(ProgramMetadataSerializerContext.Default);
        options.TypeInfoResolverChain.Add(DbmsMetadataSerializerContext.Default);
        options.TypeInfoResolverChain.Add(ProgramMetadataSerializerContext.Default);

        return options;
    }

    /// <summary>
    /// Returns a JSON string or <c>null</c>
    /// based on the presence of one of two conventional environment variables.
    /// </summary>
    /// <remarks>
    /// Of the two expected environment variables,
    /// The one with the <c>_PATH</c> suffix should lead to JSON of the form:
    ///
    /// <code>
    /// {
    ///     "ProgramMetadata": { … }
    /// }
    /// </code>
    ///
    /// …which conforms to <see cref="IConfiguration"/> conventions.
    /// </remarks>
    public static string? GetJsonForProgramMetadataFromEnvironment()
    {
        string? json = Environment.GetEnvironmentVariable(EnvVarSettings);
        string? path = Environment.GetEnvironmentVariable(EnvVarSettingsPath);

        if (string.IsNullOrWhiteSpace(json) && string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(json))
        {
            return json;
        }

        if (!string.IsNullOrWhiteSpace(path))
        {
            json = File.ReadAllText(path);
        }

        JsonElement jE = JsonElementUtility.ParseJson(json, logger: NullLogger.Instance);

        if (!jE.IsExpectedObject(NullLogger.Instance, nameof(ProgramMetadata))) return json;

        JsonElement? actual = jE
            .GetJsonChildElementOrNull(nameof(ProgramMetadata));

        return actual == null ? json : actual.Value.GetRawText();
    }

    internal const string EnvVarSettings = "SONGHAY_APP_SETTINGS";
    internal const string EnvVarSettingsPath = "SONGHAY_APP_SETTINGS_PATH";
}
