using System;
using System.Collections.ObjectModel;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ObservableCollection&lt;T&gt;" />.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Sets the collection with digits for <c>mantissaDigits = 2</c>.
        /// </summary>
        /// <param name="collectionOfByte">The collection of byte.</param>
        /// <param name="digits">The digits.</param>
        public static void SetCollectionWithDigits(this ObservableCollection<byte?> collectionOfByte, double digits)
        {
            collectionOfByte.SetCollectionWithDigits(digits, mantissaDigits: 2);
        }

        /// <summary>
        /// Sets the collection with digits.
        /// </summary>
        /// <param name="collectionOfByte">The collection of byte.</param>
        /// <param name="digits">The digits.</param>
        /// <param name="mantissaDigits">The mantissa digits.</param>
        public static void SetCollectionWithDigits(this ObservableCollection<byte?> collectionOfByte, double digits, int mantissaDigits)
        {
            if(collectionOfByte == null) return;
            var x = Convert.ToInt32(100 * MathUtility.GetMantissa(digits, mantissaDigits));
            if(collectionOfByte.Count < mantissaDigits) return;

            collectionOfByte[0] = (x >= 1e0) ? MathUtility.GetDigitInNumber(x, 1) : 0;
            collectionOfByte[1] = (x >= 1e1) ? MathUtility.GetDigitInNumber(x, 2) : 0;

            x = Convert.ToInt32(MathUtility.TruncateNumber(digits));

            for(int i = 2; i < collectionOfByte.Count; i++)
            {
                var place = i - 1;
                var y = i - 2;

                collectionOfByte[i] = (x < Math.Pow(10, y)) ? 0 : MathUtility.GetDigitInNumber(x, place);
            }
        }
    }
}
