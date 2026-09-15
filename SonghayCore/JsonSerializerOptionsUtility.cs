namespace Songhay;

/// <summary>
/// Caches frequently-used <see cref="JsonSerializerOptions"/>
/// </summary>
public static class JsonSerializerOptionsUtility
{
    /// <summary>
    /// Returns <see cref="JsonSerializerOptions"/>
    /// with <see cref="JsonSerializerOptions.WriteIndented"/> set to <c>true</c>
    /// </summary>
    public static JsonSerializerOptions JsonSerializerOptionsForIndentation { get; } = new()
    {
        WriteIndented = true
    };
}