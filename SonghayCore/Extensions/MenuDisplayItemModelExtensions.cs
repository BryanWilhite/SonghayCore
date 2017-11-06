using System;
using System.Collections.Generic;
using System.Linq;

namespace Songhay.Extensions
{
    using Models;

    /// <summary>
    /// Extensions of <see cref="MenuDisplayItemModel"/>
    /// </summary>
    public static class MenuDisplayItemModelExtensions
    {
        /// <summary>
        /// Returns the Default Selection
        /// <c>IsDefaultSelection == true</c>
        /// or the First <see cref="MenuDisplayItemModel"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public static MenuDisplayItemModel DefaultOrFirst(this IEnumerable<MenuDisplayItemModel> data)
        {
            if (data == null) return null;

            if (data.Where(i => i.IsDefaultSelection == true).Count() > 0)
            {
                return data.FirstOrDefault(i => i.IsDefaultSelection == true);
            }
            else
            {
                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the deep copy.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <remarks>
        /// This is useful in Silverlight.
        /// </remarks>
        public static MenuDisplayItemModel GetDeepCopy(this MenuDisplayItemModel data)
        {
            if (data == null) return null;

            var children = (data.ChildItems != null) ?
                data.ChildItems.Select(i => i.GetDeepCopy()).ToArray()
                :
                null;

            return new MenuDisplayItemModel
            {
                ChildItems = children,
                Description = data.Description,
                DisplayText = data.DisplayText,
                Id = data.Id,
                IsDefaultSelection = data.IsDefaultSelection,
                IsEnabled = data.IsEnabled,
                IsSelected = data.IsSelected,
                ItemCategory = data.ItemCategory,
                ItemName = data.ItemName,
                SortOrdinal = data.SortOrdinal,
                Tag = data.Tag
            };

        }

        /// <summary>
        /// Fluently returns <see cref="MenuDisplayItemModel" /> with child item.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="child">The child.</param>
        public static MenuDisplayItemModel WithChildItem(this MenuDisplayItemModel data, MenuDisplayItemModel child)
        {
            if (data == null) return null;
            if (child == null) return data;
            data.ChildItems = new MenuDisplayItemModel[] { child };
            return data;
        }

        /// <summary>
        /// Fluently returns <see cref="MenuDisplayItemModel" /> with child items.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="list">The list.</param>
        public static MenuDisplayItemModel WithChildItems(this MenuDisplayItemModel data, MenuDisplayItemModel[] list)
        {
            if (data == null) return null;
            if (list == null) return data;
            data.ChildItems = list;
            return data;
        }

        /// <summary>
        /// Fluently returns <see cref="MenuDisplayItemModel" /> with child items.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="list">The list.</param>
        public static MenuDisplayItemModel WithChildItems(this MenuDisplayItemModel data, List<MenuDisplayItemModel> list)
        {
            if (data == null) return null;
            if (list == null) return data;
            data.ChildItems = list.ToArray();
            return data;
        }

        /// <summary>
        /// Fluently returns <see cref="MenuDisplayItemModel" /> without children.
        /// </summary>
        /// <param name="data">The data.</param>
        public static MenuDisplayItemModel WithoutChildren(this MenuDisplayItemModel data)
        {
            if (data == null) return null;
            return new MenuDisplayItemModel
            {
                Description = data.Description,
                DisplayText = data.DisplayText,
                Id = data.Id,
                ItemCategory = data.ItemCategory,
                ItemName = data.ItemName,
                SortOrdinal = data.SortOrdinal
            };
        }
    }
}
