using System;
using System.Linq;
using Songhay.Extensions;
using Songhay.Models;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions
{
    public class ITemporalExtensionsTests
    {
        public ITemporalExtensionsTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
        }

        [Fact]
        public void ToJObject_Test()
        {
            var instance = new MyClass
            {
                MyStringProperty = $"{nameof(ToJObject_Test)}",
                InceptDate = DateTime.Now
            };

            var jO = instance.ToJObject<MyClass>(useJavaScriptCase: true);

            Assert.NotNull(jO);
            Assert.Equal(3, jO.Properties().Count());

            this._testOutputHelper.WriteLine(jO.ToString());
        }

        readonly ITestOutputHelper _testOutputHelper;

        class MyClass : ITemporal
        {
            public int MyIntProperty { get; set; }
            public int? MyNullableIntProperty { get; set; }
            public string MyStringProperty { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? InceptDate { get; set; }
            public DateTime? ModificationDate { get; set; }
        }
    }
}
