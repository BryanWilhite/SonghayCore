using System;

namespace Songhay
{
    /// <summary>
    /// Functor seeds
    /// </summary>
    public static class FuncSeed
    {
        /// <summary>
        /// Predicate Functor for <c>true</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static Func<T, bool> True<T>() { return f => true; }

        /// <summary>
        /// Predicate Functor for <c>false</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static Func<T, bool> False<T>() { return f => false; }
    }
}
