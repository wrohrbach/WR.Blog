using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace WR.Blog.Mvc.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Convert Foreign Accent Characters
        /// </summary>
        /// <returns>Common ASCII representation</returns>
        public static string RemoveAccent(this string s)
        {
            return Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(s));
        }

        /// <summary>
        /// Cleanses String for URL
        /// </summary>
        /// <returns>URL Friendly String</returns>
        public static string Clean(this string s)
        {
            var sb = new StringBuilder(
                Regex.Replace(
                    HttpUtility.HtmlDecode(s.Replace("&", "and"))
                                            .RemoveAccent(), "(?:[^A-Za-z0-9 ]|(?<=['\"])s)", "")
                                            .Trim()
                    );
            sb.Replace("  ", " ").Replace(" ", "-");

            return sb.ToString().ToLower();
        }
    }
}