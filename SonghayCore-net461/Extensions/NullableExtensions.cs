using System;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <c>Nullable</c> types.
    /// </summary>
    public static class NullableExtensions
    {
        /// <summary>
        /// Rounds the specified decimal.
        /// </summary>
        /// <param name="decimalValue">The decimal value.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns></returns>
        /// <remarks>
        /// For more detail see http://anderly.com/2009/08/08/silverlight-midpoint-rounding-solution/
        /// </remarks>
        public static decimal Round(this decimal? decimalValue, int decimals)
        {
            var d = decimalValue.GetValueOrDefault();
            decimal factor = Convert.ToDecimal(Math.Pow(10, decimals));
            int sign = Math.Sign(d);
            return Decimal.Truncate(d * factor + 0.5m * sign) / factor;
        }
    }
}
