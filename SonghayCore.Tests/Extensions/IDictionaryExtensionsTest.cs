using System.Collections.Specialized;

namespace Songhay.Tests.Extensions;

// ReSharper disable once InconsistentNaming
public class IDictionaryExtensionsTest(ITestOutputHelper helper)
{
    [Fact]
    public void ShouldAddAndOverwrite()
    {
        var dictionary = new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 3,
        };

        dictionary["two"] = 2;
        dictionary["three"] = 3;

        Assert.Equal(1+2+3,dictionary.Sum(pair => pair.Value));
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

        NameValueCollection set = dictionary.ToNameValueCollection();
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
            helper.WriteLine("Testing for exception...");

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
                helper.WriteLine("Exception expected.");
                hasThrownException = true;
            }

            Assert.True(hasThrownException, "The expected exception did not throw.");
        }

        void TestRef()
        {
            helper.WriteLine("Testing for reference value...");

            var dictionary = new Dictionary<int, string>
            {
                { 0, "zero" },
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };

            var actual = dictionary.TryGetValueWithKey(4);
            Assert.Null(actual);
        }

        void TestValue()
        {
            helper.WriteLine("Testing for value...");

            var dictionary = new Dictionary<string, int>
            {
                { "zero", 0 },
                { "one", 1 },
                { "two", 2 },
                { "three", 4 },
            };

            var actual = dictionary.TryGetValueWithKey("four");
            Assert.Equal(0, actual);
        }

        #endregion

        TestException();
        TestRef();
        TestValue();
    }

    [Fact]
    public void ToShallowClone_Test()
    {
        var dictionary = new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
        };

        IDictionary<string, int> actual = dictionary.ToShallowClone();

        Assert.Equal(dictionary.Sum(pair => pair.Value), actual.Sum(pair => pair.Value));
        Assert.Equal(
            dictionary.Select(pair => pair.Key).Aggregate((a,i) => $"{a}{i}"),
            actual.Select(pair => pair.Key).Aggregate((a,i) => $"{a}{i}")
        );
    }

    public static readonly TheoryData<Dictionary<string, int?>?, string> ToUriQueryString_dictionary_string_int_TestData =
        new()
        {
            {
                new Dictionary<string, int?>
                {
                    { "one", null },
                    { "two", 2 },
                },
                "?one=MISSING&two=2"
            },
            {
                new Dictionary<string, int?>(),
                string.Empty
            },
            {
                null,
                string.Empty
            },
        };

    [Theory]
    [MemberData(nameof(ToUriQueryString_dictionary_string_int_TestData))]
    public void ToUriQueryString_dictionary_string_int_Test(Dictionary<string, int?>? input, string expected)
    {
        // act:
        string actual = input.ToUriQueryString();

        helper.WriteLine($"{nameof(actual)}: `{actual}`");

        // assert:
        Assert.Equal(expected, actual);
    }

    // ReSharper disable once InconsistentNaming
    public static readonly TheoryData<Dictionary<string, string?>?, string> ToUriQueryString_dictionary_string_string_TestData =
        new()
        {
            {
                new Dictionary<string, string?>
                {
                    { "one", "uno" },
                    { "two", "this one needs to be encoded" },
                },
                "?one=uno&two=this one needs to be encoded"
            }
        };

    [Theory]
    [MemberData(nameof(ToUriQueryString_dictionary_string_string_TestData))]
    public void ToUriQueryString_dictionary_string_string_Test(Dictionary<string, string?>? input, string expected)
    {
        // act:
        string actual = input.ToUriQueryString();

        helper.WriteLine($"{nameof(actual)}: `{actual}`");

        // assert:
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void UnionAsDictionary_Test()
    {
        var dictionary1 = new Dictionary<string, int>
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
        };

        var dictionary2 = new Dictionary<string, int>
        {
            ["three"] = 36,
            ["four"] = 4,
            ["five"] = 5,
        };

        IDictionary<string, int>? actual = dictionary1.UnionAsDictionary(dictionary2);

        Assert.NotNull(actual);
        Assert.Equal(5, actual.Count);

        helper.WriteLine($"{actual["three"]}");
    }

    [Fact]
    public void WithPair_Test()
    {
        var dictionary = new Dictionary<string, int>().WithPair("one", 1);

        Assert.NotNull(dictionary);
        Assert.NotEmpty(dictionary);
        Assert.Equal("one", dictionary.First().Key);
        Assert.Equal(1, dictionary.First().Value);

        dictionary.WithPair("two", 2);

        Assert.Equal("two", dictionary.Last().Key);
        Assert.Equal(2, dictionary.Last().Value);
    }

    [Fact]
    public void WithPair_KeyValuePair_Test()
    {
        var dictionary = new Dictionary<string, int>().WithPair(new KeyValuePair<string, int>("one", 1));

        Assert.NotNull(dictionary);
        Assert.NotEmpty(dictionary);
        Assert.Equal("one", dictionary.First().Key);
        Assert.Equal(1, dictionary.First().Value);
    }

    [Fact]
    public void WithPairs_Test()
    {
        var pairs = new[] {("one", 1), ("two", 2)}
            .Select(t => new KeyValuePair<string, int>(t.Item1, t.Item2));

        var dictionary = new Dictionary<string, int>().WithPairs(pairs);

        Assert.NotNull(dictionary);
        Assert.Equal(2, dictionary.Count);
        Assert.Equal("two", dictionary.Last().Key);
        Assert.Equal(2, dictionary.Last().Value);
    }
}