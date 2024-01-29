namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="MenuDisplayItemModel"/>
/// </summary>
public static class MenuDisplayItemModelExtensions
{
    /// <summary>
    /// Gets all <see cref="MenuDisplayItemModel"/>
    /// by the specified predicate.
    /// </summary>
    /// <param name="data">the <see cref="MenuDisplayItemModel"/></param>
    /// <param name="predicate">the predicate</param>
    public static IEnumerable<MenuDisplayItemModel> GetAllBy(this MenuDisplayItemModel? data,
        Func<MenuDisplayItemModel, bool> predicate)
    {
        var items1 = data != null ?
            new[] {data}.Where(predicate)
            :
             Enumerable.Empty<MenuDisplayItemModel>();
        var items2 = data?
            .ChildItems.SelectMany(i => i.GetAllBy(predicate))
             ??
             Enumerable.Empty<MenuDisplayItemModel>();

        return items1.Union(items2);
    }

    /// <summary>
    /// Returns <c>true</c> when the grouping has the specified identifier.
    /// </summary>
    /// <param name="data">the <see cref="MenuDisplayItemModel"/></param>
    /// <param name="groupId">the <see cref="MenuDisplayItemModel.GroupId"/></param>
    public static bool HasGroupId(this MenuDisplayItemModel? data, string groupId) => data != null &&
        !string.IsNullOrWhiteSpace(groupId) && data.GroupId.EqualsInvariant(groupId);

    /// <summary>
    /// Returns the Default Selection
    /// <c>IsDefaultSelection == true</c>
    /// or the First <see cref="MenuDisplayItemModel"/>.
    /// </summary>
    /// <param name="data">The data.</param>
    public static MenuDisplayItemModel? DefaultOrFirst(this IEnumerable<MenuDisplayItemModel>? data)
    {
        if (data == null) return null;

        var snapshot = data as MenuDisplayItemModel[] ?? data.ToArray();

        return snapshot.Any(i => i.IsDefaultSelection == true)
            ? snapshot.FirstOrDefault(i => i.IsDefaultSelection == true)
            : snapshot.FirstOrDefault();
    }

    /// <summary>
    /// Fluently returns <see cref="MenuDisplayItemModel" /> with child item.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="child">The child.</param>
    public static MenuDisplayItemModel? WithChildItem(this MenuDisplayItemModel? data, MenuDisplayItemModel? child)
    {
        if (data == null) return null;
        if (child == null) return data;

        data.ChildItems = new[] {child};

        return data;
    }

    /// <summary>
    /// Fluently returns <see cref="MenuDisplayItemModel" /> with child items.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="items">The items.</param>
    public static MenuDisplayItemModel? WithChildItems(this MenuDisplayItemModel? data,
        IEnumerable<MenuDisplayItemModel>? items)
    {
        if (data == null) return null;
        if (items == null) return data;

        var snapshot = items as MenuDisplayItemModel[] ?? items.ToArray();

        data.ChildItems = snapshot.ToArray();

        return data;
    }
}
