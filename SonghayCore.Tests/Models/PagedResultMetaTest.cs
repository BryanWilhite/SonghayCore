using Songhay.Models;

namespace Songhay.Tests.Models;

public class PagedResultMetaTest
{
    [Fact]
    public void ShouldGetPageCount()
    {
        var model = new PagedResultMeta
        {
            PageIndex = 1,
            PageSize = 10,
            TotalCount = 18
        };

        Assert.Equal(2, model.PageCount);
    }
}