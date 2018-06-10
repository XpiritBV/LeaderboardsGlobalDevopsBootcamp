using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Collections.Generic;

namespace gdbcLeaderBoard.UnitTests
{
    [TestClass]
    public class TagHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tag = TagHelper.GetUniqueTag(TagHelper.GetExampleTags());

            Assert.IsTrue(tag.ToLowerInvariant() == "J4JMNKVC".ToLowerInvariant(), $"Found tag '{tag}'");
        }
    }

    /// <summary>
    /// Temp copy implementation of the taghelper method to test, because of incompatibility between dotnetcore 1.2 and .NET framework 4.6.2
    /// </summary>
    public static class TagHelper
    {
        private static List<string> IgnoreTags = new List<string> { "required", "help" };
        public static string GetUniqueTag(string from)
        {
            var splitted = from.Split(',');

            foreach (var tag in splitted)
            {
                if (IgnoreTags.IndexOf(tag.ToLowerInvariant()) == -1)
                {
                    if (tag.Length == 8 && tag.IndexOf('-') == -1)
                    {
                        return tag;
                    }
                }
            }

            return "";
        }

        public static string GetExampleTags()
        {
            var sb = new StringBuilder();

            sb.Append("F001-P004,");
            sb.Append("REQUIRED,");
            sb.Append("J4JMNKVC,");
            sb.Append("HELP");

            return sb.ToString();
        }
    }
}
