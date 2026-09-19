namespace Songhay.SerializerContexts;

/// <summary>
/// Defines an explicit <see cref="JsonSerializerContext"/>
/// for <see cref="MenuDisplayItemModel"/>
/// to avoid using reflection
/// which is not available in AOT-compilation assemblies.
/// </summary>
[JsonSerializable(typeof(MenuDisplayItemModel))]
[JsonSerializable(typeof(IEnumerable<MenuDisplayItemModel>))]
public partial class MenuDisplayItemModelSerializerContext : JsonSerializerContext;