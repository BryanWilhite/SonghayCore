using Songhay.Models;

namespace Songhay.Tests.Extensions;

public class MenuDisplayItemModelExtensionsTests
{
    // ReSharper disable once InconsistentNaming
    public static IEnumerable<object[]> GetAllByData =
    [
        [
            new MenuDisplayItemModel
                {
                    GroupId = "g-42",
                    ChildItems =
                    [
                        new MenuDisplayItemModel(),
                            new MenuDisplayItemModel { GroupId = "g-42" },
                            new MenuDisplayItemModel
                            {
                                ChildItems =
                                [
                                    new MenuDisplayItemModel { GroupId = "g-42" }
                                ]
                            }
                    ]
                },
                (MenuDisplayItemModel i) => i.HasGroupId("g-42"), // predicate
                3 // expectedNumberOfItems
        ]
    ];

    [Theory]
    [MemberData(nameof(GetAllByData))]
    public void GetAllBy_Test(MenuDisplayItemModel data, Func<MenuDisplayItemModel, bool> predicate, int expectedNumberOfItems)
    {
        var actual = data.GetAllBy(predicate).ToArray();
        Assert.NotEmpty(actual);
        Assert.Equal(expectedNumberOfItems, actual.Length);
    }
}
