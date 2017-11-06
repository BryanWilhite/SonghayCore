using System;
using System.Linq;
using System.Linq.Expressions;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Linq.Expressions.Expression"/>
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Ors the specified this expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisExpression">The this expression.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> thisExpression, Expression<Func<T, bool>> expression)
        {
            var invokedExpr = Expression.Invoke(expression, thisExpression.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(thisExpression.Body, invokedExpr), thisExpression.Parameters);
        }

        /// <summary>
        /// Ands the specified this expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thisExpression">The this expression.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> thisExpression, Expression<Func<T, bool>> expression)
        {
            var invokedExpr = Expression.Invoke(expression, thisExpression.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(thisExpression.Body, invokedExpr), thisExpression.Parameters);
        }
    }
}
