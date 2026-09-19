namespace Songhay.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="ProgramMetadata"/> to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(ProgramMetadata))]
[JsonSerializable(typeof(DbmsMetadata))]
[JsonSerializable(typeof(RestApiMetadata))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(Dictionary<string, DbmsMetadata>))]
[JsonSerializable(typeof(Dictionary<string, RestApiMetadata>))]
public partial class ProgramMetadataSerializerContext : JsonSerializerContext;
