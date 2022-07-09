using System;
using System.Collections.Generic;
using Songhay.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Tests.Extensions;

// ReSharper disable once InconsistentNaming
public class IDictionaryExtensionsTest
{
    public IDictionaryExtensionsTest(ITestOutputHelper helper)
    {
        _testOutputHelper = helper;
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

        void TestException()
        {
            _testOutputHelper.WriteLine("Testing for exception...");

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
                _testOutputHelper.WriteLine("Exception expected.");
                hasThrownException = true;
            }

            Assert.True(hasThrownException, "The expected exception did not throw.");
        }

        void TestRef()
        {
            _testOutputHelper.WriteLine("Testing for reference value...");

            var dictionary = new Dictionary<int, string>
            {
                { 0, "zero" },
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };

            var actual = dictionary.TryGetValueWithKey(4);
            Assert.Equal(default, actual);
        }

        void TestValue()
        {
            _testOutputHelper.WriteLine("Testing for value...");

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

        TestException();
        TestRef();
        TestValue();
    }

    readonly ITestOutputHelper _testOutputHelper;
}