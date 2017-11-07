using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Models;
using System;

namespace Songhay.Tests
{

    [TestClass]
    public class PagedResultMetaTest
    {
        [TestMethod]
        public void ShouldGetPageCount()
        {
            var model = new PagedResultMeta
            {
                PageIndex = 1,
                PageSize = 10,
                TotalCount = 18
            };

            Assert.AreEqual<int>(2, model.PageCount);
        }
    }
}
