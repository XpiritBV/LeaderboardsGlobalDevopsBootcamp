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

        [TestMethod]
        public void DefaultTagHelperNoSpacesTest()
        {
            var vstsTags = $"F001-P001;{DefaultUniqueTag }";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }

        [TestMethod]
        public void RequiredTagHelperTest()
        {
            var vstsTags = $"F001-P001; {DefaultUniqueTag }; REQUIRED";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }

        [TestMethod]
        public void RequiredLastTagHelperNoSpacesTest()
        {
            var vstsTags = $"F001-P001;{DefaultUniqueTag };REQUIRED";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }

        [TestMethod]
        public void RequiredFirstTagHelperTest()
        {
            var vstsTags = $"REQUIRED; F001-P001; {DefaultUniqueTag }";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }

        [TestMethod]
        public void HelpAndRequiredFirstTagHelperTest()
        {
            var vstsTags = $"Help; REQUIRED; F001-P001; {DefaultUniqueTag }";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }


        [TestMethod]
        public void RequiredSecondTagHelperTest()
        {
            var vstsTags = $"F001-P001; REQUIRED; {DefaultUniqueTag }";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }

        [TestMethod]
        public void RequiredSecondHelpTagHelperTest()
        {
            var vstsTags = $"F001-P001; REQUIRED; {DefaultUniqueTag }; Help";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }

        [TestMethod]
        public void RequiredSecondHelpUpperTagHelperTest()
        {
            var vstsTags = $"F001-P001; REQUIRED; {DefaultUniqueTag }; HELP";
            var uniqueTag = TagHelper.GetUniqueTag(vstsTags);

            Assert.IsTrue(uniqueTag == DefaultUniqueTag);
        }
    }
}
