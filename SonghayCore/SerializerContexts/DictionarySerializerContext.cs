namespace Songhay.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="Dictionary{TKey,TValue}"/>
/// where <c>TKey</c> and <c>TValue</c> are <see cref="string"/>
/// to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(Dictionary<string, string>))]
public partial class DictionarySerializerContext : JsonSerializerContext;
