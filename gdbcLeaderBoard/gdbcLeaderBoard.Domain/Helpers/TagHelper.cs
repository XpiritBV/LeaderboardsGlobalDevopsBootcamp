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
            if (string.IsNullOrWhiteSpace(from))
            {
                return "";
            }

            var splitted = from.Split(';');

            foreach (var tag in splitted)
            {
                var trimmedTag = tag.Trim().ToLowerInvariant();
                if (IgnoreTags.IndexOf(trimmedTag) == -1)
                {
                    if (trimmedTag.Length == 8 && trimmedTag.IndexOf('-') == -1)
                    {
                        return tag.Trim();
                    }
                }
            }

            return "";
        }
    }
}
