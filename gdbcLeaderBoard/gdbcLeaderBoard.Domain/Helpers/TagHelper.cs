using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdbcLeaderBoard.Helpers
{
    public static class TagHelper
    {
        private static List<string> IgnoreTags = new List<string> { "required", "help" };
        public static string GetUniqueTag(string from)
        {
            var splitted = from.Split(';');

            foreach (var tag in splitted)
            {
                if (IgnoreTags.IndexOf(tag.ToLowerInvariant()) == -1)
                {
                    if (tag.Trim().Length == 8 && tag.IndexOf('-') == -1)
                    {
                        return tag.Trim();
                    }
                }
            }

            return "";
        }
    }
}
