namespace Songhay.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="DisplayItemModel"/>
/// to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(DisplayItemModel))]
[JsonSerializable(typeof(IEnumerable<DisplayItemModel>))]
public partial class DisplayItemModelSerializerContext : JsonSerializerContext;