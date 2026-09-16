namespace Songhay;

/// <summary>
/// Caches frequently-used <see cref="JsonSerializerOptions"/>
/// </summary>
public static class JsonSerializerOptionsCache
{
    /// <summary>
    /// Returns <see cref="JsonSerializerOptions"/>
    /// with <see cref="JsonSerializerOptions.PropertyNamingPolicy"/> set
    /// to <see cref="JsonNamingPolicy.CamelCase"/> 
    /// </summary>
    public static JsonSerializerOptions OptionsForCamelCaseOnly { get; private set; } =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

    /// <summary>
    /// Returns <see cref="JsonSerializerOptions"/>
    /// with <see cref="JsonSerializerOptions.PropertyNamingPolicy"/> set
    /// to <see cref="JsonNamingPolicy.CamelCase"/> 
    /// and <see cref="JsonSerializerOptions.WriteIndented"/> set to <c>true</c>
    /// </summary>
    public static JsonSerializerOptions OptionsForCamelCaseWithIndentation { get; private set; } =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

    /// <summary>
    /// Returns <see cref="JsonSerializerOptions"/>
    /// with <see cref="JsonSerializerOptions.WriteIndented"/> set to <c>true</c>
    /// </summary>
    public static JsonSerializerOptions OptionsForIndentationOnly { get; } = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Returns <see cref="JsonSerializerOptions"/>
    /// with <see cref="JsonSerializerOptions.WriteIndented"/> set to <c>true</c>
    /// and <see cref="JsonSerializerOptions.IndentSize"/> set to <c>4</c>
    /// </summary>
    public static JsonSerializerOptions OptionsForIndentationWithSize4 { get; } = new()
    {
        IndentSize = 4,
        WriteIndented = true
    };
}
