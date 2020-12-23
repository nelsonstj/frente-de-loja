using System.Text.RegularExpressions;

namespace DV.FrenteLoja.Core.Extensions
{
    public static class Lists
    {
        public static bool Like(this string s, string pattern)
        {
            //Find the pattern anywhere in the string
            pattern = ".*" + pattern + ".*";

            return Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase);
        }
    }
}
