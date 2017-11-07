using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Songhay.Tests
{
    using Songhay;

    [TestClass]
    public class MathUtilityTest
    {
        [TestMethod]
        public void ShouldGetMantissa()
        {
            double x = -1034.068;
            double mantissaValue = MathUtility.GetMantissa(x, 2);

            Assert.AreEqual(-0.07, mantissaValue, "The expected mantissa decimal is not here");
        }

        [TestMethod]
        public void ShouldTruncateNumber()
        {
            double x = -1034.068;
            double truncated = MathUtility.TruncateNumber(x);

            Assert.AreEqual(Math.Truncate(x), truncated, "The expected truncated number is not here");
        }

        [TestMethod]
        public void ShouldGetDigitInNumber()
        {
            var x = 1324.068;
            var digit = MathUtility.GetDigitInNumber((int)(x * 1000), 3);

            Assert.AreEqual(0, digit, "The expected digit is not here.");
        }
    }
}
