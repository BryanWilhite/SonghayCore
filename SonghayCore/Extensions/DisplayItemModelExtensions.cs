using Songhay.Models;
using System;

namespace Songhay.Extensions
{

    /// <summary>
    /// Extensions of <see cref="DisplayItemModel"/>.
    /// </summary>
    public static class DisplayItemModelExtensions
    {
        /// <summary>
        /// Returns <c>true</c> when the item has the <see cref="DisplayItemModel.Tag"/>
        /// based on the specified evaluator.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="evaluator"></param>
        /// <returns></returns>
        public static bool HasTag(this DisplayItemModel data, Func<object, bool> evaluator)
        {
            if (data == null) return false;
            if (evaluator == null) return false;
            return evaluator.Invoke(data.Tag);
        }

        /// <summary>
        /// Converts the <see cref="DisplayItemModel"/> into a menu display item model.
        /// </summary>
        /// <param name="data">The data.</param>
        public static MenuDisplayItemModel ToMenuDisplayItemModel(this DisplayItemModel data)
        {
            if (data == null) return null;

            return data as MenuDisplayItemModel;
        }

        /// <summary>
        /// Fluently sets <see cref="DisplayItemModel.Tag"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static DisplayItemModel WithTag(this DisplayItemModel data, object tag)
        {
            if (data == null) return null;
            data.Tag = tag;
            return data;
        }
    }
}
