namespace Songhay.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="ColorDisplayItemModel"/>
/// to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(ColorDisplayItemModel))]
[JsonSerializable(typeof(IEnumerable<ColorDisplayItemModel>))]
public partial class ColorDisplayItemModelSerializationContext : JsonSerializerContext;