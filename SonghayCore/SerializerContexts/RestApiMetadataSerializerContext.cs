namespace Songhay.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="RestApiMetadata"/> to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(RestApiMetadata))]
public partial class RestApiMetadataSerializerContext : JsonSerializerContext;
