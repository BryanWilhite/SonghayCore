﻿using Songhay.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Songhay.Tests.Extensions
{
    public class ObservableCollectionExtensionsTest
    {
        [Fact]
        public void ShouldSetCollectionWithDigits()
        {
            var x = 10106.875129d;
            var eight = new List<byte?> { 0, 0, 0, 0, 0, 0, 0, 0 };
            var collection = new ObservableCollection<byte?>(eight);

            collection.SetCollectionWithDigits(x);
            Assert.Equal<double>(8, collection[0].Value);

            x = x * .0001d;
            collection.SetCollectionWithDigits(x);
            Assert.Equal<double>(1, collection[0].Value);

        }
    }
}
