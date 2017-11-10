using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Models;
using Songhay.Tests.Activities;
using System.IO;

namespace Songhay.Tests.Models
{
    [TestClass]
    public class ActivitiesGetterTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("basePath", "Activities")]
        public void ShouldGetActivityFromDefaultName()
        {
            var projectFolder = this.TestContext.ShouldGetAssemblyDirectoryParent(this.GetType(), expectedLevels: 3);

            #region test properties:

            var basePath = this.TestContext.Properties["basePath"].ToString();
            basePath = Path.Combine(projectFolder, basePath);
            this.TestContext.ShouldFindDirectory(basePath);

            #endregion

            var args = new[]
            {
                nameof(GetHelloWorldActivity),
                "--world-name",
                "Saturn"
            };

            var getter = new MyActivitiesGetter(args);
            var activity = getter.GetActivity();
            Assert.IsNotNull(activity);

            activity.Start(getter.Args);
        }
    }
}
