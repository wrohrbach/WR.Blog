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
        #region Url Segment String Extension
        /// <summary>
        /// Convert Foreign Accent Characters to common ASCII
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
        #endregion

        #region Shorten String Extension
        /// <summary>
        /// Shortens the string to the specified length. Optionally appends an ellipse.
        /// </summary>
        /// <param name="value">The string to be shortened.</param>
        /// <param name="length">The desired string length.</param>
        /// <param name="appendEllipse">Append an ellipse on the end if set to true.</param>
        /// <returns>Returns a shortened string</returns>
        public static string Shorten(this string value, int length, bool appendEllipse)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > length)
            {
                value = value.Substring(0, length) + (appendEllipse ? "..." : string.Empty);
            }

            return value;            
        }
        #endregion
    }
}