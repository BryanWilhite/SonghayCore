namespace Songhay.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="DbmsMetadata"/> to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(DbmsMetadata))]
public partial class DbmsMetadataSerializerContext : JsonSerializerContext;
