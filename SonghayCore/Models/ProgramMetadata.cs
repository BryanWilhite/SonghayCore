namespace Songhay.Models;

/// <summary>
/// Defines the conventional Program metadata.
/// </summary>
public class ProgramMetadata
{
    /// <summary>
    /// Gets or sets the cloud storage set.
    /// </summary>
    [Obsolete("See https://github.com/BryanWilhite/SonghayCore/issues/176")]
    public Dictionary<string, Dictionary<string, string>> CloudStorageSet { get; init; } = new();

    /// <summary>
    /// Gets or sets the DBMS set.
    /// </summary>
    public Dictionary<string, DbmsMetadata> DbmsSet { get; init; } = new();

    /// <summary>
    /// Gets or sets the REST API metadata set.
    /// </summary>
    public Dictionary<string, RestApiMetadata> RestApiMetadataSet { get; init; } = new();

    /// <summary>
    /// Represents this instance as a <c>string</c>.
    /// </summary>
    public override string ToString()
    {
        var sb = new StringBuilder();

        if (DbmsSet.Any())
        {
            sb.AppendLine($"{nameof(DbmsSet)}:");
            foreach (var item in DbmsSet)
            {
                sb.AppendLine($"    {item.Key}:");
                sb.AppendLine($"        {item.Value}");
            }
        }

        if (RestApiMetadataSet.Any())
        {
            sb.AppendLine($"{nameof(RestApiMetadataSet)}:");
            foreach (var item in RestApiMetadataSet)
            {
                sb.AppendLine($"    {item.Key}:");
                sb.AppendLine($"        {item.Value}");
            }
        }

        return sb.Length > 0 ? sb.ToString() : base.ToString() ?? GetType().Name;
    }
}
