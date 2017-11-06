using System;
using System.Linq.Expressions;

namespace Songhay
{
    /// <summary>
    /// Seeds of <see cref="System.Linq.Expressions.Expression"/>
    /// </summary>
    public static class ExpressionSeed
    {
        /// <summary>
        /// <see cref="System.Linq.Expressions.Expression"/> <c>true</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// <see cref="System.Linq.Expressions.Expression"/> <c>false</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
    }
}
