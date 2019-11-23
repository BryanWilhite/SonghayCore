using Newtonsoft.Json.Linq;
using Xunit;

namespace Songhay.Extensions.Tests
{
    class AllTheProperties : INotAllTheProperties
    {
        public string PropertyOne { get; set; }

        public string PropertyTwo { get; set; }

        public string PropertyThree { get; set; }
    }

    interface INotAllTheProperties
    {
        string PropertyOne { get; set; }

        string PropertyTwo { get; set; }
    }

    class AllThePropertiesNesting : INotAllTheProperties
    {
        public string PropertyOne { get; set; }

        public string PropertyTwo { get; set; }

        public string PropertyThree { get; set; }

        public AllThePropertiesNesting PropertyNested { get; set; }
    }

    public partial class JObjectExtensionsTests
    {
        [Fact]
        public void FromJObject_Test()
        {
            var quackLikeDuck = new
            {
                propertyOne = "one",
                propertyTwo = "two"
            };

            var jQuack = JObject.FromObject(quackLikeDuck);

            var actualDuck = jQuack.FromJObject<INotAllTheProperties, AllTheProperties>();

            Assert.NotNull(actualDuck);
            Assert.Equal(quackLikeDuck.propertyOne, actualDuck.PropertyOne);
            Assert.Equal(quackLikeDuck.propertyTwo, actualDuck.PropertyTwo);
            Assert.Null(actualDuck.PropertyThree);
        }

        [Fact]
        public void FromJObject_Nesting_Test()
        {
            var quackLikeDuck = new
            {
                propertyOne = "one",
                propertyTwo = "two",
                propertyThree = new
                {
                    propertyOne = "one-nested",
                    propertyTwo = "two-nested",
                }
            };

            var jQuack = JObject.FromObject(quackLikeDuck);

            var actualDuck = jQuack.FromJObject<INotAllTheProperties, AllThePropertiesNesting>();

            Assert.NotNull(actualDuck);
            Assert.Equal(quackLikeDuck.propertyOne, actualDuck.PropertyOne);
            Assert.Equal(quackLikeDuck.propertyTwo, actualDuck.PropertyTwo);
            Assert.Null(actualDuck.PropertyThree);
            Assert.Null(actualDuck.PropertyNested);
        }
    }
}
