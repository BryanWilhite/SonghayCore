#if NET452 || NET462

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Songhay.Collections
{
    /// <summary>
    /// Extends <see cref="System.Collections.ObjectModel.ObservableCollection{T}"/>
    /// with sorting.
    /// </summary>
    /// <typeparam name="T">Collection Type</typeparam>
    /// <remarks>
    ///     There are many ways to handle this issue. The code here is based on
    ///     “Write a Sortable ObservableCollection for WPF” by Brian Lagunas
    ///     [http://elegantcode.com/2009/05/14/write-a-sortable-observablecollection-for-wpf/]
    /// </remarks>
    public class ObservableSortingCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortingCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public ObservableSortingCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Sorts the specified key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="direction">The direction.</param>
        public void Sort<TKey>(Func<T, TKey> keySelector, ListSortDirection direction)
        {
            switch(direction)
            {
                case ListSortDirection.Ascending:
                    {
                        this.ApplySort(this.Items.OrderBy(keySelector));
                        break;
                    }
                case ListSortDirection.Descending:
                    {
                        this.ApplySort(this.Items.OrderByDescending(keySelector));
                        break;
                    }
            }
        }

        /// <summary>
        /// Sorts the specified key selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            this.ApplySort(this.Items.OrderBy(keySelector, comparer));
        }

        void ApplySort(IEnumerable<T> sortedItems)
        {
            if(sortedItems == null) return;

            var sortedItemsList = sortedItems.ToList();

            foreach(var item in sortedItemsList)
            {
                this.Move(this.IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }
    }
}

#endif
