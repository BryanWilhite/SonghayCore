namespace Songhay.Extensions;

/// <summary>
/// Extensions of <see cref="DisplayItemModel"/>.
/// </summary>
public static class DisplayItemModelExtensions
{
    /// <summary>
    /// Returns <c>true</c> when the item has the <see cref="DisplayItemModel.Tag"/>
    /// based on the specified evaluator.
    /// </summary>
    /// <param name="data">The <see cref="DisplayItemModel"/>.</param>
    /// <param name="evaluator">The predicate for evaluating <see cref="DisplayItemModel.Tag"/>.</param>
    public static bool HasTag(this DisplayItemModel? data, Func<object?, bool>? evaluator) =>
        data != null && evaluator != null && evaluator.Invoke(data.Tag);

    /// <summary>
    /// Converts the <see cref="DisplayItemModel"/> into a menu display item model.
    /// </summary>
    /// <param name="data">The <see cref="DisplayItemModel"/>.</param>
    public static MenuDisplayItemModel? ToMenuDisplayItemModel(this DisplayItemModel? data) =>
        data as MenuDisplayItemModel;

    /// <summary>
    /// Fluently sets <see cref="DisplayItemModel.Tag"/>.
    /// </summary>
    /// <param name="data">The <see cref="DisplayItemModel"/>.</param>
    /// <param name="tag">The value of <see cref="DisplayItemModel.Tag"/>.</param>
    public static DisplayItemModel? WithTag(this DisplayItemModel? data, object tag)
    {
        if (data == null) return null;

        data.Tag = tag;

        return data;
    }
}
