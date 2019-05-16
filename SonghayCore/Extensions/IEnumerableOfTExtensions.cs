using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>.
    /// </summary>
    public static class IEnumerableOfTExtensions
    {
        /// <summary>
        /// Flattens the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="childGetter">The child getter.</param>
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> childGetter)
        {
            if (source == null) return Enumerable.Empty<TSource>();
            var flattenedList = new List<TSource>(source);

            source.ForEachInEnumerable(i =>
            {
                var children = childGetter(i);
                if (children != null) flattenedList.AddRange(children.Flatten(childGetter));
            });

            return flattenedList;
        }

        /// <summary>
        /// Flattens the specified source.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="childGetter">The child getter.</param>
        /// <param name="flattenedHead">The flattened head.</param>
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> childGetter, TSource flattenedHead)
        {
            return new[] { flattenedHead }.Concat(source.Flatten(childGetter));
        }

        /// <summary>
        /// Performs the <see cref="System.Action"/>
        /// on each item in the enumerable object.
        /// </summary>
        /// <typeparam name="TEnumerable">The type of the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action.</param>
        /// <remarks>
        /// “I am philosophically opposed to providing such a method, for two reasons.
        /// …The first reason is that doing so violates the functional programming principles
        /// that all the other sequence operators are based upon. Clearly the sole purpose of a call
        /// to this method is to cause side effects.”
        /// —Eric Lippert, “foreach” vs “ForEach” [http://blogs.msdn.com/b/ericlippert/archive/2009/05/18/foreach-vs-foreach.aspx]
        /// </remarks>
        public static void ForEachInEnumerable<TEnumerable>(this IEnumerable<TEnumerable> enumerable, Action<TEnumerable> action)
        {
            if (enumerable == null) return;
            if (action == null) return;

            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// Determines whether this instance is enumerable.
        /// </summary>
        /// <typeparam name="TEnumerable">The type of the enumerable.</typeparam>
        /// <param name="data">The data.</param>
        public static bool IsEnumerableType<TEnumerable>(this object data)
        {
            return (data as IEnumerable<TEnumerable>) != null;
        }

        /// <summary>
        /// Partitions the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="size">The size.</param>
        /// <remarks>
        /// This member is by Jon Skeet.
        /// [http://stackoverflow.com/questions/438188/split-a-collection-into-n-parts-with-linq]
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
        {
            T[] array = null;
            int count = 0;
            foreach (T item in source)
            {
                if (array == null)
                {
                    array = new T[size];
                }
                array[count] = item;
                count++;
                if (count == size)
                {
                    yield return new ReadOnlyCollection<T>(array);
                    array = null;
                    count = 0;
                }
            }
            if (array != null)
            {
                Array.Resize(ref array, count);
                yield return new ReadOnlyCollection<T>(array);
            }
        }

        /// <summary>
        /// Projects the previous item with the current item.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="projection">The projection.</param>
        /// <returns></returns>
        /// <remarks>
        /// “This enables you to perform your projection using only a single pass of the source sequence,
        /// which is always a bonus (imagine running it over a large log file).
        /// Note that it will project a sequence of length n into a sequence of length n-1—
        /// you may want to prepend a ‘dummy’ first element, for example. (Or change the method to include one.)
        /// Here’s an example of how you'd use it:
        /// <code>
        /// var query = list.SelectWithPrevious((prev, cur) =&gt; new { ID = cur.ID, Date = cur.Date, DateDiff = (cur.Date - prev.Date).Days);
        /// </code>
        /// Note that this will include the final result of one ID with the first result of the next ID…
        /// you may wish to group your sequence by ID first.”
        /// —Jon Skeet, “Calculate difference from previous item with LINQ”
        /// [http://stackoverflow.com/questions/3683105/calculate-difference-from-previous-item-with-linq/3683217#3683217]
        /// </remarks>
        public static IEnumerable<TResult> SelectWithPrevious<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> projection)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    yield break;
                }
                TSource previous = iterator.Current;
                while (iterator.MoveNext())
                {
                    yield return projection(previous, iterator.Current);
                    previous = iterator.Current;
                }
            }
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable{TEnumerable}"/>
        /// into <see cref="ICollection{TEnumerable}"/>.
        /// </summary>
        /// <typeparam name="TEnumerable"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        /// <remarks>
        /// For details, see “When To Use IEnumerable, ICollection, IList And List”
        /// [http://www.claudiobernasconi.ch/2013/07/22/when-to-use-ienumerable-icollection-ilist-and-list/]
        /// </remarks>
        public static ICollection<TEnumerable> ToCollection<TEnumerable>(this IEnumerable<TEnumerable> enumerable)
        {
            if (enumerable == null) return Enumerable.Empty<TEnumerable>().ToList();
            return enumerable.ToList();
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable{TSource}"/> into a display string.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="data">The data.</param>
        public static string ToDisplayString<TSource>(this IEnumerable<TSource> data) where TSource : class
        {
            return data.ToDisplayString<TSource>(indent: 0);
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable{TSource}"/> into a display string.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="data">The data.</param>
        /// <param name="indent">The indent.</param>
        public static string ToDisplayString<TSource>(this IEnumerable<TSource> data, byte indent) where TSource : class
        {
            var indentation = string.Join(string.Empty, Enumerable.Repeat(" ", indent).ToArray());
            var builder = new StringBuilder();

            if ((data != null) && (data.Any())) builder.Append($"{indentation}{data.Count()} child items:");
            data.ForEachInEnumerable(i => builder.Append($"{Environment.NewLine}{indentation}{i}"));
            if (builder.Length > 0) builder.AppendLine();

            return builder.ToString();
        }
    }
}