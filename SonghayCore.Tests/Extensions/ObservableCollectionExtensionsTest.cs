using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Songhay.Tests.Extensions
{
    using Songhay.Extensions;

    [TestClass]
    public class ObservableCollectionExtensionsTest
    {
        [TestMethod]
        public void ShouldSetCollectionWithDigits()
        {
            var x = 10106.875129d;
            var eight = new List<byte?> { 0, 0, 0, 0, 0, 0, 0, 0 };
            var collection = new ObservableCollection<byte?>(eight);

            collection.SetCollectionWithDigits(x);
            Assert.AreEqual<double>(8, collection[0].Value, "The expected digit is not here.");

            x = x * .0001d;
            collection.SetCollectionWithDigits(x);
            Assert.AreEqual<double>(1, collection[0].Value, "The expected digit is not here.");

        }
    }
}
