using Songhay.Extensions;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions;

public class IDictionaryExtensionsTest
{
    public IDictionaryExtensionsTest(ITestOutputHelper helper)
    {
        this._testOutputHelper = helper;
    }

    [Fact]
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
        Assert.True(set.Count > 0, "The expected set items are not here.");

        /*
            And now the tests that any decent tester would remove:
        */

        var setPair = new KeyValuePair<string, string>("3", "three again"); // shows that NameValueCollection is all about strings
        set.Add(setPair.Key, setPair.Value); // shows that KeyValuePair has little to do with NameValueCollection

        Assert.Equal(dictionary.Count, set.Count); // shows that NameValueCollection acts like the set abstract data type

        Assert.Equal(string.Concat(dictionary[3], ",", setPair.Value), set[setPair.Key]); // shows how NameValueCollection concatenates to keep unique keys
    }

    [Fact]
    public void ShouldTryGetValueWithKey()
    {
        #region local functions:

        void testException()
        {
            this._testOutputHelper.WriteLine("Testing for exception...");

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
                this._testOutputHelper.WriteLine("Exception expected.");
                hasThrownException = true;
            }

            Assert.True(hasThrownException, "The expected exception did not throw.");
        }

        void testRef()
        {
            this._testOutputHelper.WriteLine("Testing for refence value...");

            var dictionary = new Dictionary<int, string>
            {
                { 0, "zero" },
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };

            var actual = dictionary.TryGetValueWithKey(4);
            Assert.Equal(default(string), actual);
        }

        void testValue()
        {
            this._testOutputHelper.WriteLine("Testing for value...");

            var dictionary = new Dictionary<string, int>
            {
                { "zero", 0 },
                { "one", 1 },
                { "two", 2 },
                { "three", 4 },
            };

            var actual = dictionary.TryGetValueWithKey("four");
            Assert.Equal(default(int), actual);
        }

        #endregion

        testException();
        testRef();
        testValue();
    }

    readonly ITestOutputHelper _testOutputHelper;
}