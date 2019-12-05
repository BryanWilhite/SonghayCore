using System;
using Songhay.Extensions;
using Songhay.Models;
using Xunit;

namespace Songhay.Tests.Extensions
{
    public class ObjectExtensionsTests
    {
        [Fact]
        public void IsSerializable_Test()
        {
            var actual = Activator
                .CreateInstance(typeof(OpmlDocument))
                .IsSerializable<OpmlDocument>(throwException: false);
            Assert.True(actual);
        }
    }
}
