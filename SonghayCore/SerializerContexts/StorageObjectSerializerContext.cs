namespace Songhay.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="StorageObject"/>
/// to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(StorageObject))]
[JsonSerializable(typeof(IEnumerable<StorageObject>))]
public partial class StorageObjectSerializerContext : JsonSerializerContext;
