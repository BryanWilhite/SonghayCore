using System;
using System.Collections.Generic;
using System.Linq;

namespace Songhay.Extensions
{
    using Models;

    /// <summary>
    /// Extensions of <see cref="DisplayItemModel"/>.
    /// </summary>
    public static class DisplayItemModelExtensions
    {
        /// <summary>
        /// Determines whether <see cref="DisplayItemModel.ItemCategory"/>
        /// is <see cref="DisplayItemModelCategories.GenericWebDocument"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public static bool HasGenericWebDocumentCategory(this DisplayItemModel data)
        {
            if (data == null) return false;
            return (data.ItemCategory == DisplayItemModelCategories.GenericWebDocument);
        }

        /// <summary>
        /// Determines whether <see cref="DisplayItemModel.ItemCategory"/>
        /// is <see cref="DisplayItemModelCategories.GenericWebFragment"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public static bool HasGenericWebFragmentCategory(this DisplayItemModel data)
        {
            if (data == null) return false;
            return (data.ItemCategory == DisplayItemModelCategories.GenericWebFragment);
        }

        /// <summary>
        /// Determines whether [has generic web log category] [the specified data].
        /// </summary>
        /// <param name="data">The data.</param>
        public static bool HasGenericWebLogCategory(this DisplayItemModel data)
        {
            if (data == null) return false;
            return (data.ItemCategory == DisplayItemModelCategories.GenericWebLog);
        }

        /// <summary>
        /// Determines whether [has generic web schema category] [the specified data].
        /// </summary>
        /// <param name="data">The data.</param>
        public static bool HasGenericWebSchemaCategory(this DisplayItemModel data)
        {
            if (data == null) return false;
            return (data.ItemCategory == DisplayItemModelCategories.GenericWebSchema);
        }

        /// <summary>
        /// Determines whether <see cref="DisplayItemModel.ItemCategory"/>
        /// is <see cref="DisplayItemModelCategories.GenericWebSegment"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public static bool HasGenericWebSegmentCategory(this DisplayItemModel data)
        {
            if (data == null) return false;
            return (data.ItemCategory == DisplayItemModelCategories.GenericWebSegment);
        }

#if !NETSTANDARD1_2 && !NETSTANDARD1_4
        /// <summary>
        /// Converts the <see cref="DisplayItemModel"/> into a menu display item model.
        /// </summary>
        /// <param name="data">The data.</param>
        public static MenuDisplayItemModel ToMenuDisplayItemModel(this DisplayItemModel data)
        {
            if (data == null) return null;

            var menuItem = new MenuDisplayItemModel();
            FrameworkTypeUtility.SetProperties(data, menuItem);

            return menuItem;
        }
#endif

        /// <summary>
        /// Fluently sets <see cref="DisplayItemModel.ItemCategory"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="itemCategory">The item category.</param>
        public static DisplayItemModel WithItemCategory(this DisplayItemModel data, string itemCategory)
        {
            if (data == null) return null;
            data.ItemCategory = itemCategory;
            return data;
        }

        /// <summary>
        /// Fluently sets <see cref="DisplayItemModel.Tag"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static DisplayItemModel WithTag(this DisplayItemModel data, string tag)
        {
            if (data == null) return null;
            data.Tag = tag;
            return data;
        }
    }
}
