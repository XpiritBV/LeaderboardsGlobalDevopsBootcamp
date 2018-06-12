using gdbcLeaderBoard.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace gdbcLeaderBoard.UnitTests
{
    [TestClass]
    public class TagHelperTest
    {
        private static string DefaultUniqueTag = "XU9BQNU0";

        [TestMethod]
        public void DefaultTagHelperTest()
        {
            var vstsTags = $"F001-P001; {DefaultUniqueTag }";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }
    }
}
