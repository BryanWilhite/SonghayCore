using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System;
using System.Collections.Generic;

namespace Songhay.Tests.Extensions
{
    [TestClass]
    public class IDictionaryExtensionsTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldConvertToNameValueCollection()
        {
            var dictionary = new Dictionary<int, string>
            {
                { 0, "zero" },
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };

            var set = dictionary.ToNameValueCollection();
            Assert.IsTrue(set.Count > 0, "The expected set items are not here.");

            /*
                And now the tests that any decent tester would remove:
            */

            var setPair = new KeyValuePair<string, string>("3", "three again"); // shows that NameValueCollection is all about strings
            set.Add(setPair.Key, setPair.Value); // shows that KeyValuePair has little to do with NameValueCollection

            Assert.AreEqual(dictionary.Count, set.Count,
                "Equal dictionary and set counts were expected."); // shows that NameValueCollection acts like the set abstract data type

            Assert.AreEqual(string.Concat(dictionary[3], ",", setPair.Value), set[setPair.Key],
                "The expected set concatenation is not here."); // shows how NameValueCollection concatenates to keep unique keys
        }

        [TestMethod]
        public void ShouldTryGetValueWithKey()
        {
            #region local functions:

            void testException()
            {
                this.TestContext.WriteLine("Testing for exception...");

                var hasThrownException = false;

                var dictionary = new Dictionary<string, string>
                {
                    { "uno", "one" },
                    { "dos", "two" },
                    { "tres", "three" },
                };
                try
                {
                    dictionary.TryGetValueWithKey("quatro", throwException: true);
                }
                catch (NullReferenceException)
                {
                    this.TestContext.WriteLine("Exception expected.");
                    hasThrownException = true;
                }

                Assert.IsTrue(hasThrownException, "The expected exception did not throw.");
            }

            void testRef()
            {
                this.TestContext.WriteLine("Testing for refence value...");

                var dictionary = new Dictionary<int, string>
                {
                    { 0, "zero" },
                    { 1, "one" },
                    { 2, "two" },
                    { 3, "three" },
                };

                var actual = dictionary.TryGetValueWithKey(4);
                Assert.AreEqual(default(string), actual, $"The expected default value {typeof(string)} is not here.");
            }

            void testValue()
            {
                this.TestContext.WriteLine("Testing for value...");

                var dictionary = new Dictionary<string, int>
                {
                    { "zero", 0 },
                    { "one", 1 },
                    { "two", 2 },
                    { "three", 4 },
                };

                var actual = dictionary.TryGetValueWithKey("four");
                Assert.AreEqual(default(int), actual, $"The expected default value {typeof(int)} is not here.");
            }

            #endregion

            testException();
            testRef();
            testValue();
        }
    }
}
