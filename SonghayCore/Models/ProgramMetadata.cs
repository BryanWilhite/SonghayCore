namespace Songhay.Models;

/// <summary>
/// Defines the conventional Program metadata.
/// </summary>
public class ProgramMetadata
{
    /// <summary>
    /// Gets or sets the cloud storage set.
    /// </summary>
    /// <value>
    /// The cloud storage set.
    /// </value>
    public Dictionary<string, Dictionary<string, string>> CloudStorageSet { get; set; } = new();

    /// <summary>
    /// Gets or sets the DBMS set.
    /// </summary>
    /// <value>
    /// The DBMS set.
    /// </value>
    public Dictionary<string, DbmsMetadata> DbmsSet { get; set; } = new();

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    /// <summary>
    /// Gets or sets the REST API metadata set.
    /// </summary>
    /// <value>
    /// The API metadata set.
    /// </value>
    public Dictionary<string, RestApiMetadata> RestApiMetadataSet { get; set; } = new();

    /// <summary>
    /// Represents this instance as a <c>string</c>.
    /// </summary>
    public override string ToString()
    {
        var sb = new StringBuilder();

        if (CloudStorageSet.Any())
        {
            sb.AppendLine($"{nameof(CloudStorageSet)}:");
            foreach (var item in CloudStorageSet)
            {
                sb.AppendLine($"    {item.Key}:");
                if (item.Value.Any())
                {
                    foreach (var item2 in item.Value)
                    {
                        var maxLength = 64;
                        if (item2.Value.Length >= maxLength)
                            sb.AppendLine($"        {item2.Key}: {item2.Value.Substring(0, maxLength)}... ");
                        else
                            sb.AppendLine($"        {item2.Key}: {item2.Value}... ");
                    }
                }
            }
        }

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
