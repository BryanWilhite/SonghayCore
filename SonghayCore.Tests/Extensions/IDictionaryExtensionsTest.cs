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
            Assert.Equal(default, actual);
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
            Assert.Equal(default(int), actual);
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

        var actual = dictionary.ToShallowClone();

        Assert.Equal(dictionary.Sum(pair => pair.Value), actual.Sum(pair => pair.Value));
        Assert.Equal(
            dictionary.Select(pair => pair.Key).Aggregate((a,i) => $"{a}{i}"),
            actual.Select(pair => pair.Key).Aggregate((a,i) => $"{a}{i}")
        );
    }

    [Fact]
    public void WithPair_Test()
    {
        var dictionary = new Dictionary<string, int>().WithPair("one", 1);

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

        Assert.Equal(2, dictionary.Count);
        Assert.Equal("two", dictionary.Last().Key);
        Assert.Equal(2, dictionary.Last().Value);
    }
}