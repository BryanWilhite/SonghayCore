using System.Text.Json.Serialization;

namespace Songhay.S3.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="Dictionary{TKey,TValue}"/>
/// where <c>TKey</c> and <c>TValue</c> are <see cref="string"/>
/// to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(StorageObject))]
[JsonSerializable(typeof(IReadOnlyCollection<StorageObject>))]
public partial class StorageObjectSerializerContext : JsonSerializerContext;
