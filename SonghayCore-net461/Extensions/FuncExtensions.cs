using System;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Func&lt;T&gt;"/>.
    /// </summary>
    public static class FuncExtensions
    {
        /// <summary>
        /// Ands the specified <see cref="System.Func&lt;T&gt;"/> predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisPredicate">The extended <see cref="System.Func&lt;T&gt;"/> predicate.</param>
        /// <param name="predicate">The composed <see cref="System.Func&lt;T&gt;"/> predicate.</param>
        public static Func<T, bool> And<T>(this Func<T, bool> thisPredicate, Func<T, bool> predicate)
        {
            return a => thisPredicate(a) && predicate(a);
        }

        /// <summary>
        /// Ands the specified <see cref="System.Func&lt;T&gt;"/> predicate with negation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisPredicate">The extended <see cref="System.Func&lt;T&gt;"/> predicate.</param>
        /// <param name="predicate">The composed, negated <see cref="System.Func&lt;T&gt;"/> predicate.</param>
        public static Func<T, bool> AndNot<T>(this Func<T, bool> thisPredicate, Func<T, bool> predicate)
        {
            return a => thisPredicate(a) && !predicate(a);
        }

        /// <summary>
        /// Ors the specified <see cref="System.Func&lt;T&gt;"/> predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisPredicate">The extended <see cref="System.Func&lt;T&gt;"/> predicate.</param>
        /// <param name="predicate">The composed <see cref="System.Func&lt;T&gt;"/> predicate.</param>
        public static Func<T, bool> Or<T>(this Func<T, bool> thisPredicate, Func<T, bool> predicate)
        {
            return a => thisPredicate(a) || predicate(a);
        }

        /// <summary>
        /// Ors the specified <see cref="System.Func&lt;T&gt;"/> predicate with negation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisPredicate">The extended <see cref="System.Func&lt;T&gt;"/> predicate.</param>
        /// <param name="predicate">The composed, negated <see cref="System.Func&lt;T&gt;"/> predicate.</param>
        public static Func<T, bool> OrNot<T>(this Func<T, bool> thisPredicate, Func<T, bool> predicate)
        {
            return a => thisPredicate(a) || !predicate(a);
        }
    }
}
