using System;
using System.Collections.Generic;
using System.Text;

namespace Songhay
{
    /// <summary>
    /// Static helpers for Math.
    /// </summary>
    public static partial class MathUtility
    {
        /// <summary>
        /// Gets the digit in number.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="place">The place (from right to left).</param>
        public static byte? GetDigitInNumber(int x, int place)
        {
            var d = (int)Math.Pow(10, place - 1);
            if(d < 0) return null;
            return Convert.ToByte((x / d) % 10);
        }

        /// <summary>
        /// Gets the mantissa.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="round">The round.</param>
        public static double GetMantissa(double x, int round)
        {
            double mantissaValue = ((x < 0) ? -1 : 1) *
                (Math.Abs(x) - Math.Floor(Math.Abs(x)));

            mantissaValue = Math.Round(mantissaValue, round);

            return mantissaValue;
        }

        /// <summary>
        /// Truncates the number.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <remarks>
        /// Silverlight does not have <c>Math.Truncate()</c>.
        /// </remarks>
        public static double TruncateNumber(double x)
        {
            return ((x < 0) ? -1 : 1) * Math.Floor(Math.Abs(x));
        }
    }
}
